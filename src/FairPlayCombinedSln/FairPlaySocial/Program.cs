using Blazored.Toast;
using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.Extensions;
using FairPlayCombined.Services.FairPlaySocial;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.Post;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlaySocial.ClientServices;
using FairPlaySocial.Components;
using FairPlaySocial.Components.Account;
using FairPlaySocial.Data;
using FairPlaySocial.MinimalApiEndpoints;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddSmtpClient(Constants.ConnectionStringNames.MailDev);

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


builder.Services.AddAuthentication(configureOptions =>
{
    configureOptions.DefaultScheme = IdentityConstants.ApplicationScheme;
    configureOptions.DefaultSignInScheme = IdentityConstants.ExternalScheme;
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
Extensions.EnhanceConnectionString(nameof(FairPlaySocial), ref connectionString);
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

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});

builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddBlazoredToast();
builder.AddIdentityEmailSender();
builder.Services.AddTransient<ICultureService, CultureService>();
builder.Services.AddTransient<HttpClientService>();
builder.Services.AddTransient<PostService>();
builder.Services.AddTransient<IPhotoService, PhotoService>();

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

await app.UseDatabaseDrivenLocalization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();
// Add additional endpoints required by the Identity /Account Razor components.
app.MapIdentityApi<ApplicationUser>();
app.MapAdditionalIdentityEndpoints();
app.MapClientAppsEndpoints(clientAppsAuthPolicy);
app.MapHub<PostNotificationHub>(FairPlayCombined.Common.FairPlaySocial.Constants.Hubs.HomeFeedHub);
app.MapHub<UserMessageNotificationHub>(FairPlayCombined.Common.FairPlaySocial.Constants.Hubs.UserMessageHub);

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();
