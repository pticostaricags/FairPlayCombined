
using FairPlayAdminPortal.Components;
using FairPlayAdminPortal.Data;
using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.Extensions;
using FairPlayCombined.Services.FairPlayDating;
using FairPlayCombined.SharedAuth.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
if (Convert.ToBoolean(builder.Configuration["UseSendGrid"]))
{
    builder.AddSmtpClient(Constants.ConnectionStringNames.SMTP);
}
builder.Services.AddFluentUIComponents();
builder.Services.AddDatabaseDrivenLocalization();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayAdminPortal), ref connectionString);
builder.AddSqlServerDbContext<ApplicationDbContext>(connectionName: "FairPlayCombinedDb");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
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
}, contextLifetime: ServiceLifetime.Transient, optionsLifetime: ServiceLifetime.Transient);

builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>();
builder.EnrichSqlServerDbContext<FairPlayCombinedDbContext>();
builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICustomCache, CustomCache>();
builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.AddIdentityEmailSender();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ApplicationUserVouchService>();
builder.Services.AddTransient<ResourceService>();
builder.Services.AddTransient<ConfigurationSecretService>();
builder.Services.AddTransient<OpenAIPromptService>();
builder.Services.AddTransient<IPromptGeneratorService, PromptGeneratorService>();
builder.Services.AddTransient<IUserFundsUniqueCodesService, UserFundsUniqueCodesService>();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
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
    .AddAdditionalAssemblies(FairPlayAdminPortal.UIConfiguration.AdditionalSetup.AdditionalAssemblies);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

await app.RunAsync();

namespace Utilities
{
    public static class ThemesHelper
    {
        public static DesignThemeModes DesignThemeMode { get; set; } = DesignThemeModes.Light;
        public static OfficeColor? OfficeColor { get; set; } = Microsoft.FluentUI.AspNetCore.Components.OfficeColor.Default;
    }
}