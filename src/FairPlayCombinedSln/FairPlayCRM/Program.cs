using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FairPlayCRM.Components;
using FairPlayCRM.Data;
using FairPlayCombined.Common;
using FairPlayCombined.Services.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.AspNetCore.ResponseCompression;
using System.IO.Compression;
using FairPlayCombined.Models.GoogleAuth;
using FairPlayCombined.Services.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Common.Identity;
using FairPlayCRM.Extensions;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.IpData;
using FairPlayCombined.SharedAuth.Components.Account;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

if (Convert.ToBoolean(builder.Configuration["UseSendGrid"]))
{
    builder.AddSmtpClient(Constants.ConnectionStringNames.SMTP);
}

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
Extensions.EnhanceConnectionString(nameof(FairPlayCRM), ref connectionString);
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

builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.AddIdentityEmailSender();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddPlatformServices(builder);

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

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(FairPlayCRM.UIConfiguration.AdditionalSetup.AdditionalAssemblies);

app.MapControllers();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapIdentityApi<ApplicationUser>();
app.MapAdditionalIdentityEndpoints();
app.AddLocalizationEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();

static void AddPlatformServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<ICustomCache, CustomCache>();
    builder.Services.AddSingleton<IpDataConfiguration>(sp =>
    {
        var ipDataKey = builder.Configuration["IpDataKey"] ??
        throw new InvalidOperationException("'IpDataKey' not found");
        return new IpDataConfiguration()
        {
            Key = ipDataKey,
        };
    });
    builder.Services.AddTransient<IpDataService>();
    builder.Services.AddTransient<IVisitorTrackingService, VisitorTrackingService>();
    builder.Services.AddTransient<IContactService, ContactService>();
    builder.Services.AddTransient<ICompanyService, CompanyService>();
    builder.Services.AddTransient<ILinkedInConnectionService, LinkedInConnectionService>();
}