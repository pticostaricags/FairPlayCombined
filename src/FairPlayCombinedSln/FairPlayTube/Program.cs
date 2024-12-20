using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.GoogleAuth;
using FairPlayCombined.Models.LinkedInAuth;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.Extensions;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using FairPlayCombined.SharedAuth.Components.Account;
using FairPlayCombined.SharedAuth.Extensions;
using FairPlayTube.BackgroundServices;
using FairPlayTube.Components;
using FairPlayTube.Data;
using FairPlayTube.Extensions;
using FairPlayTube.HealthChecks;
using FairPlayTube.Metrics;
using Google.Apis.YouTube.v3;
using Hangfire;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using OpenTelemetry.Metrics;
using SixLabors.ImageSharp;
using System.IO.Compression;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();



builder.AddAzureBlobClient("blobs");

if (Convert.ToBoolean(builder.Configuration["UseSendGrid"]))
{
    builder.AddSmtpClient(Constants.ConnectionStringNames.SMTP);
}
builder.Services.AddHealthChecks().AddCheck<FairPlayTubeHealthCheck>(nameof(FairPlayTubeHealthCheck),
    failureStatus: HealthStatus.Unhealthy,
    tags: ["live"]);
builder.Services.ConfigureOpenTelemetryMeterProvider((sp, meterBuilder) =>
{
    meterBuilder.AddMeter(IFairPlayTubeMetrics.SESSION_METER_NAME);
});
builder.Services.AddTransient<IFairPlayTubeMetrics, FairPlayTubeMetrics>();
builder.Services.AddFluentUIComponents();
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
          ["application/octet-stream"]);
    //Check https://learn.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-8.0#response-compression
    opts.EnableForHttps = true;
    opts.Providers.Add<BrotliCompressionProvider>();
    opts.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.Fastest;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});
builder.Services.AddSignalR();
builder.Services.AddDatabaseDrivenLocalization();
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

GoogleAuthClientSecretInfo googleAuthClientSecretInfo = builder.GetGoogleAuthClientSecretInfo();

builder.Services.AddSingleton<YouTubeClientServiceConfiguration>(new YouTubeClientServiceConfiguration()
{
    GoogleAuthClientSecretInfo = googleAuthClientSecretInfo
});

LinkedInAuthClientSecretInfo linkedInAuthClientSecretInfo = builder.GetLinkedInAuthClientSecretInfo();

var twitterClientId = builder.Configuration["TwitterClientId"];
var twitterClientSecret = builder.Configuration["TwitterClientSecret"];

var facebookAppId = builder.Configuration["FacebookAppId"];
var facebookAppSecret = builder.Configuration["FacebookAppSecret"];

builder.AddPayPalCore();

builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultScheme = IdentityConstants.ApplicationScheme;
    configureOptions.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddGoogleAuth(googleAuthClientSecretInfo,
    scopes:
    [
        YouTubeService.Scope.YoutubeUpload,
        YouTubeService.Scope.YoutubeForceSsl,
        YouTubeService.Scope.Youtubepartner
        ], true)
    .AddLinkedInAuth(linkedInAuthClientSecretInfo,
    scopes: 
    [
        "w_member_social"
        ], saveTokens:true)
    .AddTwitter(configureOptions => 
    {
        configureOptions.ClientId = twitterClientId!;
        configureOptions.ClientSecret = twitterClientSecret!;
        configureOptions.SaveTokens = true;
    })
    .AddFacebook(options => 
    {
        options.AppId = facebookAppId!;
        options.AppSecret = facebookAppSecret!;
        options.Scope.Add("email");
        options.Scope.Add("public_profile");
        options.SaveTokens = true;
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

builder.AddOpenAI();
builder.AddGoogleGemini();
builder.AddAzureAIContentSafety();

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});

builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.AddIdentityEmailSender();
builder.AddPlatformServices(googleAuthClientSecretInfo);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHangfire(options =>
{
    options.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseSqlServerStorage(connectionString);
});

builder.Services.AddHangfireServer();

builder.Services.AddHostedService<AudienceGrowthBackgroundService>();

var app = builder.Build();
ConfigureCustomValidationAttributes(app.Services);
ConfigureModelsLocalizers(app.Services);
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

app.MapStaticAssets();
app.UseAntiforgery();

app.UseHangfireDashboard();
await app.UseDatabaseDrivenLocalization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(FairPlayTube.UIConfiguration.AdditionalSetup.AdditionalAssemblies);

app.MapControllers();
// Add additional endpoints required by the Identity /Account Razor components.
app.MapIdentityApi<ApplicationUser>();
app.MapAdditionalIdentityEndpoints();
app.MapHub<UserMessageNotificationHub>(Constants.Routes.SignalRHubs.UserMessageHub);
app.AddLocalizationEndpoints();
app.AddVideoInfoEndpoints();
app.AddCustomIdentityEndpoints(clientAppsAuthPolicy);
app.MapGet("/Account/MyVideoDataExport/{videoInfoId}",
    async (
        [FromServices] IFairPlayTubeUserDataService fairPlayTubeUserDataService,
        long videoInfoId,
        CancellationToken cancellationToken
    ) =>
    {
        var data = await fairPlayTubeUserDataService.GetMyVideoDataAsync(videoInfoId, cancellationToken);
        return TypedResults.File(data, System.Net.Mime.MediaTypeNames.Application.Octet,
            "fairplaytubedata.zip");
    });
app.MapGet("/api/photo/{photoId}",
    async (
        [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        [FromRoute] long photoId,
        CancellationToken cancellationToken) =>
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.Photo
        .AsNoTracking()
        .AsSplitQuery()
        .Where(p => p.PhotoId == photoId)
        .Select(p=>p.PhotoBytes)
        .SingleAsync(cancellationToken);
        return TypedResults.File(result, System.Net.Mime.MediaTypeNames.Image.Png);
    });
app.MapGet("/api/video/{videoId}/description",
    async (
        [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        [FromRoute] string videoId,
        CancellationToken cancellationToken) =>
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.VideoInfo
        .AsNoTracking()
        .Where(p => p.VideoId == videoId)
        .Select(p => p.Description)
        .SingleOrDefaultAsync(cancellationToken);
        return TypedResults.Content(result);
    });
app.MapGet("/api/video/{videoId}/title",
    async (
        [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        [FromRoute] string videoId,
        CancellationToken cancellationToken) =>
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.VideoInfo
        .AsNoTracking()
        .Where(p => p.VideoId == videoId)
        .Select(p => p.Name)
        .SingleOrDefaultAsync(cancellationToken);
        return TypedResults.Content(result);
    });
app.MapGet("/api/video/{videoId}/thumbnail",
    async (
        [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        [FromRoute] string videoId,
        CancellationToken cancellationToken) =>
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.VideoInfo
        .AsNoTracking()
        .Where(p => p.VideoId == videoId)
        .Select(p=>p.VideoThumbnailPhoto.PhotoBytes)
        .SingleAsync(cancellationToken);
        return TypedResults.File(result, System.Net.Mime.MediaTypeNames.Image.Jpeg);
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
        .AsNoTracking()
        .Include(p => p.VideoInfo)
        .Where(p => p.VideoInfo.VideoId == videoId &&
        p.Language == language)
        .Select(p => p.Content)
        .SingleOrDefaultAsync(cancellationToken);
        return TypedResults.Content(result, System.Net.Mime.MediaTypeNames.Text.Plain);
    });

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();

static void ConfigureCustomValidationAttributes(IServiceProvider services)
{
    //Find a way to use Source Generators for this in order to avoid using reflection
    var modelsAssembly = typeof(CustomRequiredAttribute).Assembly;
    var typesWithLocalizerAttribute =
        modelsAssembly.GetTypes()
        .Where(p =>
        p.CustomAttributes
        .Any(x => x.AttributeType.Name.Contains("LocalizerOfTAttribute")))
        .ToList();
    var localizerFactory = services.GetRequiredService<IStringLocalizerFactory>();
    foreach (var singleLocalizerType in typesWithLocalizerAttribute)
    {
        var newLocalizerInstance = localizerFactory.Create(singleLocalizerType);
        var field = singleLocalizerType
            .GetProperty("Localizer", BindingFlags.Public | BindingFlags.Static);
        field!.SetValue(null, newLocalizerInstance);
    }
}
static void ConfigureModelsLocalizers(IServiceProvider services)
{
    //Find a way to use Source Generators for this in order to avoid using reflection
    var modelsAssembly = typeof(FairPlayCombined.Models.UserModel).Assembly;
    var typesWithLocalizerAttribute =
        modelsAssembly.GetTypes()
        .Where(p => 
        p.CustomAttributes
        .Any(x => x.AttributeType.Name.Contains("LocalizerOfTAttribute")))
        .ToList();
    var localizerFactory = services.GetRequiredService<IStringLocalizerFactory>();
    foreach (var singleLocalizerType in typesWithLocalizerAttribute)
    {
        var newLocalizerInstance = localizerFactory.Create(singleLocalizerType);
        var field = singleLocalizerType
            .GetProperty("Localizer", BindingFlags.Public | BindingFlags.Static);
        field!.SetValue(null, newLocalizerInstance);
    }
}