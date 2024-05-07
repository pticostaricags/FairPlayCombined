using Azure;
using Azure.AI.ContentSafety;
using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.GoogleAuth;
using FairPlayCombined.Models.GoogleGemini;
using FairPlayCombined.Models.OpenAI;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayDating;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using FairPlayCombined.Services.FairPlayTube;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlayTube.Components;
using FairPlayTube.Components.Account;
using FairPlayTube.Data;
using FairPlayTube.MetricsConfiguration;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using OpenTelemetry.Metrics;
using System.Diagnostics.Metrics;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.ConfigureOpenTelemetryMeterProvider((sp, meterBuilder) =>
{
    meterBuilder.AddMeter(FairPlayTubeMetrics.SESSION_METER_NAME);
});
builder.Services.AddTransient<FairPlayTubeMetrics>();
builder.Services.AddFluentUIComponents();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          ["application/octet-stream"]);
});
builder.Services.AddSignalR();
builder.Services.AddTransient<IStringLocalizerFactory, EFStringLocalizerFactory>();
builder.Services.AddTransient<IStringLocalizer, EFStringLocalizer>();
builder.Services.AddLocalization();
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

var googleAuthClientId = builder.Configuration["GoogleAuthClientId"] ??
        throw new InvalidOperationException("'GoogleAuthClientId' not found");

var googleAuthProjectId = builder.Configuration["GoogleAuthProjectId"] ??
        throw new InvalidOperationException("'GoogleAuthProjectId' not found");

var googleAuthUri = builder.Configuration["GoogleAuthUri"] ??
        throw new InvalidOperationException("'GoogleAuthUri' not found");

var googleAuthTokenUri = builder.Configuration["GoogleAuthTokenUri"] ??
        throw new InvalidOperationException("'GoogleAuthTokenUri' not found");

var googleAuthProviderCertUri = builder.Configuration["GoogleAuthProviderCertUri"] ??
        throw new InvalidOperationException("'GoogleAuthProviderCertUri' not found");

var googleAuthClientSecret = builder.Configuration["GoogleAuthClientSecret"] ??
        throw new InvalidOperationException("'GoogleAuthClientSecret' not found");

var googleAuthRedirectUri = builder.Configuration["GoogleAuthRedirectUri"] ??
        throw new InvalidOperationException("'GoogleAuthRedirectUri' not found");

var paypalClientId = builder.Configuration["PayPal:ClientId"] ??
    throw new InvalidOperationException("'PayPal:ClientId' not found");

var paypalClientSecret = builder.Configuration["PayPal:ClientSecret"] ??
    throw new InvalidOperationException("'PayPal:ClientSecret' not found");

builder.Services.AddTransient<PayPal.Core.PayPalHttpClient>(sp => 
{
    if (builder.Environment.IsDevelopment())
    {
        PayPal.Core.SandboxEnvironment sandboxEnvironment = new(paypalClientId, paypalClientSecret);
        PayPal.Core.PayPalHttpClient payPalHttpClient = new(sandboxEnvironment);
        return payPalHttpClient;
    }
    else
    {
        PayPal.Core.PayPalEnvironment payPalEnvironment = new(paypalClientId, paypalClientSecret,
            "/", "/");
        PayPal.Core.PayPalHttpClient payPalHttpClient = new(payPalEnvironment);
        return payPalHttpClient;
    }
});
builder.Services.AddTransient<PayPalOrderService>(sp =>
{
    var payPalHttpClient = sp.GetRequiredService<PayPal.Core.PayPalHttpClient>();
    var logger = sp.GetRequiredService<ILogger<PayPalOrderService>>();
    PayPalOrderService payPalOrderService = new(payPalHttpClient, logger);
    return payPalOrderService;
});

GoogleAuthClientSecretInfo googleAuthClientSecretInfo = new()
{
    installed = new Installed()
    {
        auth_provider_x509_cert_url = googleAuthProviderCertUri,
        auth_uri = googleAuthUri,
        client_id = googleAuthClientId,
        client_secret = googleAuthClientSecret,
        project_id = googleAuthProjectId,
        redirect_uris = [googleAuthRedirectUri],
        token_uri = googleAuthTokenUri
    }
};
builder.Services.AddSingleton<YouTubeClientServiceConfiguration>(new YouTubeClientServiceConfiguration()
{
    GoogleAuthClientSecretInfo = googleAuthClientSecretInfo
});

builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultScheme = IdentityConstants.ApplicationScheme;
    configureOptions.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddGoogle(options =>
    {
        options.ClientId = googleAuthClientSecretInfo.installed!.client_id!;
        options.ClientSecret = googleAuthClientSecretInfo.installed.client_secret!;
        options.Scope.Add(YouTubeService.Scope.YoutubeUpload);
        options.Scope.Add(YouTubeService.Scope.YoutubeForceSsl);
        options.Scope.Add(YouTubeService.Scope.Youtubepartner);
    })
    .AddBearerToken(IdentityConstants.BearerScheme)
    .AddIdentityCookies();

var clientAppsAuthPolicy = "ClientAppsAuthPolicy";
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(clientAppsAuthPolicy, policy =>
    {
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes(IdentityConstants.BearerScheme);
    });

var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayTube), ref connectionString);
builder.AddSqlServerDbContext<ApplicationDbContext>(connectionName: "FairPlayCombinedDb");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddApiEndpoints()
    .AddDefaultTokenProviders();


builder.Services.AddTransient<IUserProviderService, UserProviderService>();
builder.Services.AddDbContext<FairPlayCombinedDbContext>((sp, builder) =>
{
    IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();

    builder.AddInterceptors(new SaveChangesInterceptor(userProviderService));
    builder.UseSqlServer(connectionString,
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.UseNetTopologySuite();
            sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
}, contextLifetime: ServiceLifetime.Transient,
optionsLifetime: ServiceLifetime.Transient);

builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>();
builder.EnrichSqlServerDbContext<FairPlayCombinedDbContext>();

builder.Services.AddTransient<OpenAIService>(sp =>
{
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory =
    sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
    var dbContext = dbContextFactory.CreateDbContext();
    var openAIKeyEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name == Constants.ConfigurationSecretsKeys.OPENAI_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.OPENAI_KEY} in database");
    var generateDall3ImageUrlEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name == Constants.ConfigurationSecretsKeys.GENERATE_DALL3_IMAGE_URL_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.GENERATE_DALL3_IMAGE_URL_KEY} in database");
    var openAIChatCompletionEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name == Constants.ConfigurationSecretsKeys.OPENAI_CHAT_COMPLETION_URL_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.OPENAI_CHAT_COMPLETION_URL_KEY} in database");
    var timeoutMinutes = 3;
    HttpClient openAIAuthorizedHttpClient = new()
    {
        Timeout = TimeSpan.FromMinutes(timeoutMinutes)
    };
    openAIAuthorizedHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
        "Bearer", openAIKeyEntity.Value);
    HttpClient genericHttpClient = new()
    {
        Timeout = TimeSpan.FromMinutes(timeoutMinutes)
    };
    return new OpenAIService(openAIAuthorizedHttpClient,
        genericHttpClient: genericHttpClient, new OpenAIServiceConfiguration()
        {
            Key = openAIKeyEntity.Value,
            GenerateDall3ImageUrl = generateDall3ImageUrlEntity.Value,
            ChatCompletionsUrl = openAIChatCompletionEntity.Value
        },
    dbContextFactory: dbContextFactory);
});

builder.Services.AddSingleton<GoogleGeminiConfiguration>(sp =>
{
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory =
    sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
    var dbContext = dbContextFactory.CreateDbContext();
    var googleGeminiKeyEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name == Constants.ConfigurationSecretsKeys.GOOGLE_GEMINI_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.GOOGLE_GEMINI_KEY_KEY} in database");
    return new GoogleGeminiConfiguration()
    {
        Key = googleGeminiKeyEntity.Value
    };
});

builder.Services.AddTransient<ContentSafetyClient>(sp =>
{
    var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();

    var azureContentSafetyEndpoint = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY} in database");

    var azureContentSafetyKey = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY} in database");


    ContentSafetyClient contentSafetyClient = new(new Uri(azureContentSafetyEndpoint.Value),
                new AzureKeyCredential(azureContentSafetyKey.Value));
    return contentSafetyClient;
});
builder.Services.AddTransient<AzureContentSafetyService>(sp =>
{
    ContentSafetyClient contentSafetyClient = sp.GetRequiredService<ContentSafetyClient>();
    AzureContentSafetyService azureContentSafetyService = new(contentSafetyClient);
    return azureContentSafetyService;
});

builder.Services.AddTransient<GoogleGeminiService>(sp =>
{
    GoogleGeminiConfiguration googleGeminiConfiguration = sp.GetRequiredService<GoogleGeminiConfiguration>();
    HttpClient httpClient = new()
    {
        Timeout = TimeSpan.FromMinutes(3)
    };
    return new GoogleGeminiService(googleGeminiConfiguration, httpClient);
});

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});

builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
AddPlatformServices(builder, googleAuthClientSecretInfo);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//Check https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/signalr?view=aspnetcore-9.0#disable-response-compression-for-hot-reload
if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();
}
app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseExceptionHandler();
}
else
{
    app.UseExceptionHandler();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

using var scope = app.Services.CreateScope();
using var ctx = scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();

var supportedCultures = ctx.Culture.Select(p => p.Name).ToArray();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
// Add additional endpoints required by the Identity /Account Razor components.
app.MapIdentityApi<ApplicationUser>();
app.MapAdditionalIdentityEndpoints();
app.MapHub<UserMessageNotificationHub>(Constants.Routes.SignalRHubs.UserMessageHub);
app.MapGet("/api/video/{videoId}/thumbnail",
    async (
        [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        [FromRoute] string videoId,
        CancellationToken cancellationToken) =>
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.VideoInfo
        .Include(p => p.VideoThumbnailPhoto)
        .Where(p => p.VideoId == videoId)
        .SingleOrDefaultAsync(cancellationToken);
        return TypedResults.File(result!.VideoThumbnailPhoto.PhotoBytes, System.Net.Mime.MediaTypeNames.Image.Jpeg);
    });
app.MapGet("/api/video/{videoId}/captions/{language}",
    async (
        [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        [FromRoute] string videoId,
        [FromRoute] string language,
        CancellationToken cancellationToken) =>
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.VideoCaptions
        .Include(p => p.VideoInfo)
        .Where(p => p.VideoInfo.VideoId == videoId &&
        p.Language == language)
        .Select(p => p.Content)
        .SingleOrDefaultAsync(cancellationToken);
        return TypedResults.Content(result, System.Net.Mime.MediaTypeNames.Text.Plain);
    });

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

static void AddPlatformServices(WebApplicationBuilder builder, GoogleAuthClientSecretInfo googleAuthClientSecretInfo)
{
    builder.Services.AddTransient<ICultureService, CultureService>();
    builder.Services.AddTransient<AzureVideoIndexerServiceConfiguration>(sp =>
    {
        var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
        var azureVideoIndexerAccountIdEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
        Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_ACCOUNT_ID_KEY} in database");
        var azureVideoIndexerLocationEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
        Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_LOCATION_KEY} in database");
        var azureVideoIndexerResourceGroupEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
        Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_GROUP_KEY} in database");
        var azureVideoIndexerResourceNameEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
        Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY} in database");
        var azureVideoIndexerSubscriptionIdEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
        Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY} in database");
        AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration = new()
        {
            AccountId = azureVideoIndexerAccountIdEntity.Value,
            IsArmAccount = true,
            Location = azureVideoIndexerLocationEntity.Value,
            ResourceGroup = azureVideoIndexerResourceGroupEntity.Value,
            ResourceName = azureVideoIndexerResourceNameEntity.Value,
            SubscriptionId = azureVideoIndexerSubscriptionIdEntity.Value
        };
        return azureVideoIndexerServiceConfiguration;
    });
    builder.Services.AddTransient(sp =>
    {
        var azureVideoIndexerServiceConfiguration = sp.GetRequiredService<AzureVideoIndexerServiceConfiguration>();
        return new AzureVideoIndexerService(azureVideoIndexerServiceConfiguration,
            new HttpClient());
    });
    builder.Services.AddTransient<VideoInfoService>();
    builder.Services.AddSingleton<ClientSecrets>(new ClientSecrets()
    {
        ClientId = googleAuthClientSecretInfo.installed!.client_id,
        ClientSecret = googleAuthClientSecretInfo.installed.client_secret
    });
    builder.Services.AddTransient<YouTubeClientService>();
    builder.Services.AddTransient<VideoCaptionsService>();
    builder.Services.AddTransient<VideoDigitalMarketingPlanService>();
    builder.Services.AddTransient<VideoDigitalMarketingDailyPostsService>();
    builder.Services.AddTransient<VideoPlanService>();
    builder.Services.AddTransient<PromptGeneratorService>();
    builder.Services.AddTransient<VideoWatchTimeService>();
    builder.Services.AddTransient<SupportedLanguageService>();
    builder.Services.AddTransient<VideoViewerService>();
    builder.Services.AddTransient<UserMessageService>();
    builder.Services.AddTransient<VideoThumbnailService>();
    builder.Services.AddTransient<PhotoService>();
    builder.Services.AddTransient<VideoCommentService>();
    builder.Services.AddTransient<AspNetUsersService>();
    builder.Services.AddTransient<PayPalOrderService>();
    builder.Services.AddTransient<UserProfileService>();
    builder.Services.AddTransient<UserFundService>();
}