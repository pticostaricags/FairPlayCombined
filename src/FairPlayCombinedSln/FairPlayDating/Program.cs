using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FairPlayDating.Components;
using FairPlayDating.Components.Account;
using FairPlayDating.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Diagnostics;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.AspNetCore.Mvc;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Services.FairPlayDating;
using Blazored.Toast;
using FairPlayDating.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Localization;
using FairPlayCombined.Shared.CustomLocalization.EF;
using Azure.AI.OpenAI;
using FairPlayCombined.Common.Identity;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using FairPlayDating.Properties;

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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
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
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
            });
    });

builder.Services.AddProblemDetails();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var azureOpenAIKey = builder.Configuration["AzureOpenAIKey"] ??
    throw new InvalidOperationException("'AzureOpenAIKey' not found");
var azureOpenAIEndpoint = builder.Configuration["AzureOpenAIEndpoint"] ??
    throw new InvalidOperationException("'AzureOpenAIEndpoint' not found");


builder.Services.AddTransient<OpenAIClient>((sp) => 
{
    OpenAIClient openAIClient = new OpenAIClient(endpoint: new Uri(azureOpenAIEndpoint),
        keyCredential: new Azure.AzureKeyCredential(azureOpenAIKey));
    return openAIClient;
});
builder.Services.AddTransient<AzureOpenAIService>();

var azureContentModeratorEndpoint = builder.Configuration["AzureContentModeratorEndpoint"] ??
    throw new InvalidOperationException("'AzureContentModeratorEndpoint' not found");
var azureContentModeratorKey = builder.Configuration["AzureContentModeratorKey"] ??
    throw new InvalidOperationException("'AzureContentModeratorKey' not found");
builder.Services.AddTransient<ContentModeratorClient>(sp => 
{
    ContentModeratorClient contentModeratorClient =
                new ContentModeratorClient(new ApiKeyServiceClientCredentials(azureContentModeratorKey));
    contentModeratorClient.Endpoint = azureContentModeratorEndpoint;
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
    var photoEntity = await dbContext.Photo.AsNoTracking().SingleAsync(p=>p.PhotoId == photoId);
    var mimeType = MediaTypeNames.Image.Png;
    return Results.File(photoEntity.PhotoBytes, contentType: mimeType);
});

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

using var scope = app.Services.CreateScope();
using var ctx = scope.ServiceProvider.GetRequiredService<FairPlayCombinedDbContext>();
if (!await ctx.Database.CanConnectAsync())
    await ctx.Database.EnsureCreatedAsync();
var transaction = 
    await ctx.Database
    .BeginTransactionAsync(isolationLevel: System.Data.IsolationLevel.RepeatableRead);
await ctx.Database.ExecuteSqlRawAsync(Resources._1_Script_PostDeployment1);
await ctx.Database.ExecuteSqlRawAsync(Resources._2_FairPlayDating);
await transaction.CommitAsync();
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

