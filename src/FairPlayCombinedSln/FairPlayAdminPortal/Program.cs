
using FairPlayAdminPortal.Components;
using FairPlayAdminPortal.Components.Account;
using FairPlayAdminPortal.Data;
using FairPlayCombined.Common.EmailSenders;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayDating;
using FairPlayCombined.Shared.CustomLocalization.EF;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Mail;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddSingleton<SmtpClient>(sp =>
{
    var smtpUri = new Uri(builder.Configuration.GetConnectionString("maildev")!);

    var smtpClient = new SmtpClient(smtpUri.Host, smtpUri.Port);

    return smtpClient;
});

builder.Services.AddFluentUIComponents();
builder.Services.AddTransient<IStringLocalizerFactory, EFStringLocalizerFactory>();
builder.Services.AddTransient<IStringLocalizer, EFStringLocalizer>();
builder.Services.AddLocalization();

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
builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityMailDevEmailSender>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ApplicationUserVouchService>();
builder.Services.AddTransient<ResourceService>();
builder.Services.AddTransient<ConfigurationSecretService>();
builder.Services.AddTransient<OpenAIPromptService>();
builder.Services.AddTransient<IPromptGeneratorService, PromptGeneratorService>();
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

using var scope = app.Services.CreateScope();
using var ctx = scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();

var supportedCultures = await ctx.Culture.Select(p => p.Name).ToArrayAsync();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

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