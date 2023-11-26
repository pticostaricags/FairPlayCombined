using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FairPlayAdminPortal.Components;
using FairPlayAdminPortal.Components.Account;
using FairPlayAdminPortal.Data;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Localization;
using FairPlayCombined.Shared.CustomLocalization.EF;
using Blazored.Toast;
using FairPlayCombined.DataAccess.Interceptors;
using Microsoft.Extensions.Options;
using FairPlayCombined.Services.FairPlayDating;
using FairPlayCombined.Common.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddBlazoredToast();
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

var connectionString = Environment.GetEnvironmentVariable("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayAdminPortal), ref connectionString);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString),
    contextLifetime: ServiceLifetime.Transient,
    optionsLifetime: ServiceLifetime.Transient);
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
.AddDefaultTokenProviders();


builder.Services.AddTransient<IUserProviderService, UserProviderService>();
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(
    (sp,optionsAction) =>
    {
        IUserProviderService userProviderService = sp.GetRequiredService<IUserProviderService>();
        optionsAction.AddInterceptors(new SaveChangesInterceptor(userProviderService));
        optionsAction.UseSqlServer(connectionString,
            sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.UseNetTopologySuite();
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
            });
    });

builder.Services.AddMemoryCache();
builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ApplicationUserVouchService>();
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
var supportedCultures = ctx.Culture.Select(p => p.Name).ToArray();
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
