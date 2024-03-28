using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.OpenAI;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayTube;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlayTube.Components;
using FairPlayTube.Components.Account;
using FairPlayTube.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddFluentUIComponents();

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

var googleAuthClientId = Environment.GetEnvironmentVariable("GoogleAuthClientId") ??
    throw new InvalidOperationException("'GoogleAuthClientId' not found");
var googleAuthClientSecret = Environment.GetEnvironmentVariable("GoogleAuthClientSecret") ??
    throw new InvalidOperationException("'GoogleAuthClientSecret' not found");
var googleAuthClientSecretsFilePath = Environment.GetEnvironmentVariable("GoogleAuthClientSecretsFilePath") ??
    throw new InvalidOperationException("'GoogleAuthClientSecretsFilePath' not found");
builder.Services.AddSingleton<YouTubeClientServiceConfiguration>(new YouTubeClientServiceConfiguration()
{
    ClientSecretsFilePath = googleAuthClientSecretsFilePath
});

builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultScheme = IdentityConstants.ApplicationScheme;
    configureOptions.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddGoogle(options =>
    {
        options.ClientId = googleAuthClientId;
        options.ClientSecret = googleAuthClientSecret;
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
builder.Services.AddTransient<DbContextOptions<FairPlayCombinedDbContext>>(sp =>
{
    IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();
    DbContextOptionsBuilder<FairPlayCombinedDbContext> optionsBuilder = new();
    optionsBuilder.AddInterceptors(new SaveChangesInterceptor(userProviderService));
    optionsBuilder.UseSqlServer(connectionString,
        sqlServerOptionsAction =>
        {
            sqlServerOptionsAction.UseNetTopologySuite();
            sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });
    return optionsBuilder.Options;
});
builder.AddSqlServerDbContext<FairPlayCombinedDbContext>(connectionName: "FairPlayCombinedDb");
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>();

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

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});

builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
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
    Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_RESOURCE_NAME_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_VIDEOINDEXER_SUBSCRIPTION_ID_KEY} in database");
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
    ClientId = googleAuthClientId,
    ClientSecret = googleAuthClientSecret
});
builder.Services.AddTransient<YouTubeClientService>();
builder.Services.AddTransient<VideoCaptionsService>();
builder.Services.AddTransient<VideoDigitalMarketingPlanService>();
builder.Services.AddTransient<VideoDigitalMarketingDailyPostsService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
