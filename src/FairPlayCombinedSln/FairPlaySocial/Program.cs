using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FairPlaySocial.Components;
using FairPlaySocial.Components.Account;
using FairPlaySocial.Data;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Services.FairPlaySocial;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.Post;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using Blazored.Toast;
using FairPlaySocial.ClientServices;
using Microsoft.Extensions.Localization;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlaySocial.MinimalApiEndpoints;
using FairPlayCombined.Common.Identity;

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

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});

builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddBlazoredToast();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<ICultureService, CultureService>();
builder.Services.AddTransient<HttpClientService>();
builder.Services.AddTransient<PostService>();
builder.Services.AddTransient<PhotoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseGlobalExceptionHandler();
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

await Task.Delay(TimeSpan.FromSeconds(60));
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
app.MapClientAppsEndpoints(clientAppsAuthPolicy);
app.MapHub<PostNotificationHub>(FairPlayCombined.Common.FairPlaySocial.Constants.Hubs.HomeFeedHub);
app.MapHub<UserMessageNotificationHub>(FairPlayCombined.Common.FairPlaySocial.Constants.Hubs.UserMessageHub);

app.UseSwagger();
app.UseSwaggerUI();

app.Run();
