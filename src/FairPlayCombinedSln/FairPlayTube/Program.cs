using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FairPlayTube.Components;
using FairPlayTube.Components.Account;
using FairPlayTube.Data;
using FairPlayCombined.Common.Identity;
using Blazored.Toast;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using Microsoft.Extensions.Localization;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlayCombined.Services.FairPlayTube;
using Microsoft.AspNetCore.Mvc;
using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

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

var azureVideoIndexerAccountId = Environment.GetEnvironmentVariable("AzureVideoIndexerAccountId") ??
    throw new InvalidOperationException("'AzureVideoIndexerAccountId' not found");
var azureVideoIndexerLocation = Environment.GetEnvironmentVariable("AzureVideoIndexerLocation") ??
    throw new InvalidOperationException("'AzureVideoIndexerLocation' not found");
var azureVideoIndexerResourceGroup = Environment.GetEnvironmentVariable("AzureVideoIndexerResourceGroup") ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceGroup' not found");
var azureVideoIndexerResourceName = Environment.GetEnvironmentVariable("AzureVideoIndexerResourceName") ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceName' not found");
var azureVideoIndexerSubscriptionId = Environment.GetEnvironmentVariable("AzureVideoIndexerSubscriptionId") ??
    throw new InvalidOperationException("'AzureVideoIndexerSubscriptionId' not found");
AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration = new()
{
    AccountId = azureVideoIndexerAccountId,
    IsArmAccount = true,
    Location = azureVideoIndexerLocation,
    ResourceGroup = azureVideoIndexerResourceGroup,
    ResourceName = azureVideoIndexerResourceName,
    SubscriptionId = azureVideoIndexerSubscriptionId,
};
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

var openAIKey = builder.Configuration["OpenAIKey"] ??
    throw new InvalidOperationException("'OpenAIKey' not found");

var generateDall3ImageUrl = builder.Configuration["GenerateDall3ImageUrl"] ??
    throw new InvalidOperationException("'GenerateDall3ImageUrl' not found");

var openAIChatCompletionsUrl = builder.Configuration["OpenAIChatCompletionsUrl"] ??
    throw new InvalidOperationException("'OpenAIChatCompletionsUrl' not found");

builder.Services.AddTransient<OpenAIService>(sp => 
{
    HttpClient openAIAuthorizedHttpClient = new HttpClient();
    openAIAuthorizedHttpClient.Timeout = TimeSpan.FromMinutes(2);
    openAIAuthorizedHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
        "Bearer", openAIKey);
    HttpClient genericHttpClient = new HttpClient();
    genericHttpClient.Timeout = TimeSpan.FromMinutes(2);
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory =
    sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
    return new OpenAIService(openAIAuthorizedHttpClient,
        genericHttpClient: genericHttpClient, new OpenAIServiceConfiguration()
    {
        GenerateDall3ImageUrl = generateDall3ImageUrl,
        ChatCompletionsUrl = openAIChatCompletionsUrl
    },
    dbContextFactory:dbContextFactory);
});

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});
builder.Services.AddSingleton(azureVideoIndexerServiceConfiguration);
builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<ICultureService, CultureService>();
builder.Services.AddTransient(sp =>
{
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

await Task.Delay(TimeSpan.FromSeconds(10));
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
        .Include(p=>p.VideoInfo)
        .Where(p => p.VideoInfo.VideoId == videoId &&
        p.Language == language)
        .Select(p => p.Content)
        .SingleOrDefaultAsync(cancellationToken);
        return TypedResults.Content(result, System.Net.Mime.MediaTypeNames.Text.Plain);
    });

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
