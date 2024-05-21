using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlayDating;
using FairPlayCombined.Shared.CustomLocalization.EF;
using FairPlayDating.Components;
using FairPlayDating.Components.Account;
using FairPlayDating.Data;
using FairPlayDating.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net.Mime;
using FairPlayCombined.Services.Extensions;
using Microsoft.FluentUI.AspNetCore.Components;
using FairPlayDating.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHealthChecks().AddCheck<FairPlayDatingHealthCheck>(nameof(FairPlayDatingHealthCheck),
    failureStatus: HealthStatus.Unhealthy,
    tags: ["live"]);
builder.Services.AddFluentUIComponents();
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

var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
Extensions.EnhanceConnectionString(nameof(FairPlayDating), ref connectionString);
builder.AddSqlServerDbContext<ApplicationDbContext>(connectionName: "FairPlayCombinedDb");
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
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
}, contextLifetime: ServiceLifetime.Transient,
optionsLifetime: ServiceLifetime.Transient);

builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>();
builder.EnrichSqlServerDbContext<FairPlayCombinedDbContext>();

builder.Services.AddProblemDetails();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AddPlatformServices(builder);
var app = builder.Build();

app.MapDefaultEndpoints();
app.MapGet("api/photoimage/{photoId}", async (
    [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
    CancellationToken cancellationToken,
    long photoId) =>
{
    var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
    var photoEntity = await dbContext.Photo.AsNoTracking().SingleAsync(p => p.PhotoId == photoId);
    var mimeType = MediaTypeNames.Image.Png;
    return Results.File(photoEntity.PhotoBytes, contentType: mimeType);
});

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

app.MapControllers();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();

static void AddPlatformServices(WebApplicationBuilder builder)
{
    builder.Services.AddAzureOpenAIService();
    builder.AddOpenAI();
    builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
    builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
    builder.Services.AddTransient<ICultureService, CultureService>();
    builder.Services.AddTransient<GenderService>();
    builder.Services.AddTransient<DateObjectiveService>();
    builder.Services.AddTransient<EyesColorService>();
    builder.Services.AddTransient<HairColorService>();
    builder.Services.AddTransient<KidStatusService>();
    builder.Services.AddTransient<ReligionService>();
    builder.Services.AddTransient<TattooStatusService>();
    builder.Services.AddTransient<UserProfileService>();
    builder.Services.AddTransient<PhotoService>();
    builder.Services.AddTransient<MyMatchesService>();
    builder.Services.AddTransient<GeoNamesService>();
    builder.Services.AddTransient<IGeoLocationService, BlazorGeoLocationService>();
    builder.Services.AddTransient<PromptGeneratorService>();
    builder.Services.AddTransient<ProfessionService>();
    builder.Services.AddTransient<FrequencyService>();
    builder.Services.AddTransient<ActivityService>();
    builder.Services.AddTransient<LikedUserProfileService>();
    builder.Services.AddTransient<NotLikedUserProfileService>();
    builder.AddAzureAIContentSafety();
}