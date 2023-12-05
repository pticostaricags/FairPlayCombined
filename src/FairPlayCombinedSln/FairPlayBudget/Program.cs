using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FairPlayBudget.Components;
using FairPlayBudget.Components.Account;
using FairPlayBudget.Data;
using Microsoft.Extensions.Localization;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlayCombined.Interfaces;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Services.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Common.Identity;
using Blazored.Toast;

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

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = Environment.GetEnvironmentVariable("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayBudget), ref connectionString);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IUserProviderService, UserProviderService>();
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(
    (sp, optionsAction) =>
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


builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<ICultureService, CultureService>();
var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseGlobalExceptionHandler();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseGlobalExceptionHandler();
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
