using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.GoogleAuth;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.Extensions;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using FairPlayTube.Components;
using FairPlayTube.Components.Account;
using FairPlayTube.Data;
using FairPlayTube.Extensions;
using FairPlayTube.HealthChecks;
using FairPlayTube.MetricsConfiguration;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FluentUI.AspNetCore.Components;
using OpenTelemetry.Metrics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System.IO.Compression;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
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

builder.AddPayPalCore();

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

await app.UseDatabaseDrivenLocalization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(FairPlayTube.UIConfiguration.AdditionalSetup.AdditionalAssemblies);

app.MapControllers();
// Add additional endpoints required by the Identity /Account Razor components.
app.MapIdentityApi<ApplicationUser>();
app.MapAdditionalIdentityEndpoints();
app.MapHub<UserMessageNotificationHub>(Constants.Routes.SignalRHubs.UserMessageHub);
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
        .SingleOrDefaultAsync(cancellationToken);
        using MemoryStream inputStream = new(result!.PhotoBytes);
        using var image = await Image.LoadAsync(inputStream, cancellationToken);
        image.Mutate(operation => 
        {
            operation.Resize(options:new()
            {
                Size=new(width:1024, height:768),
                Mode = ResizeMode.Max
            });
        });
        using MemoryStream outputStream = new();
        PngEncoder pngEncoder = new()
        {
            CompressionLevel = PngCompressionLevel.BestCompression
        };
        await image.SaveAsPngAsync(outputStream, pngEncoder, cancellationToken);
        return TypedResults.File(outputStream.ToArray(), System.Net.Mime.MediaTypeNames.Image.Png);
    });
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

await app.RunAsync();

namespace FairPlayTube.UIConfiguration
{
    public static class AdditionalSetup
    {
        internal static readonly Assembly[] AdditionalAssemblies =
                [typeof(FairPlayTube.SharedUI.Components.Pages.Home).Assembly];
    }
}