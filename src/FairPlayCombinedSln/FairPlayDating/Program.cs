using Blazored.Toast;
using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureOpenAI;
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
using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Net.Mime;
using FairPlayCombined.Services.Extensions;

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

builder.Services.AddProblemDetails();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<AzureOpenAIServiceConfiguration>(sp =>
{
    var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
    var dbContext = dbContextFactory.CreateDbContext();
    var azureOpenAIEndpointEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_OPENAI_ENDPOINT_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_OPENAI_ENDPOINT_KEY} in database");
    var azureOpenAIKeyEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_OPENAI_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_OPENAI_KEY_KEY} in database");
    var azureOpenAIDeploymentNameEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_OPENAI_DEPLOYMENT_NAME_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_OPENAI_DEPLOYMENT_NAME_KEY} in database");
    AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration = new()
    {
        DeploymentName = azureOpenAIDeploymentNameEntity.Value,
        Endpoint = azureOpenAIEndpointEntity.Value,
        Key = azureOpenAIKeyEntity.Value
    };
    return azureOpenAIServiceConfiguration;
});
builder.Services.AddAzureOpenAIService();

builder.Services.AddTransient<ContentModeratorClient>(sp =>
{
    var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
    var azureContentModeratorEndpointEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_CONTENT_MODERATOR_ENDPOINT_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_MODERATOR_ENDPOINT_KEY} in database");
    var azureContentModeratorKeyEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_CONTENT_MODERATOR_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_MODERATOR_KEY_KEY} in database");
    ContentModeratorClient contentModeratorClient =
                new(new ApiKeyServiceClientCredentials(azureContentModeratorKeyEntity.Value))
                {
                    Endpoint = azureContentModeratorEndpointEntity.Value
                };
    return contentModeratorClient;
});
builder.Services.AddTransient<AzureContentModeratorService>();
builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddTransient<ICultureService, CultureService>();
builder.Services.AddBlazoredToast();
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
builder.Services.AddTransient<IGeoLocationService, BlazorGeoLocationService>();
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
app.MapAdditionalIdentityEndpoints();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

