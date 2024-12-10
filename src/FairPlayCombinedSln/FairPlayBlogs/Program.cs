using FairPlayBlogs.Components;
using FairPlayCombined.Common;
using FairPlayCombined.Common.Identity;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Interceptors;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.Extensions;
using FairPlayCombined.SharedAuth.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.FluentUI.AspNetCore.Components;
using System.IO.Compression;
using Microsoft.EntityFrameworkCore;
using FairPlayBlogs.Data;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.IpData;
using FairPlayCombined.Common.CustomAttributes;
using Microsoft.Extensions.Localization;
using System.Reflection;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Services.FairPlayBlogs;
using Microsoft.AspNetCore.Mvc;

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
Extensions.EnhanceConnectionString(nameof(FairPlayBlogs), ref connectionString);
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


builder.AddOpenAI();
builder.AddGoogleGemini();
builder.AddAzureAIContentSafety();

builder.Services.AddSignalR(hubOptions =>
{
    hubOptions.MaximumReceiveMessageSize = 20 * 1024 * 1024;
});

builder.Services.AddTransient<UserManager<ApplicationUser>, CustomUserManager>();
builder.AddIdentityEmailSender();

AddPlatformServices(builder);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

ConfigureCustomValidationAttributes(app.Services);
ConfigureModelsLocalizers(app.Services);

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

app.MapStaticAssets();
app.UseAntiforgery();

await app.UseDatabaseDrivenLocalization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(FairPlayBlogs.UIConfiguration.AdditionalSetup.AdditionalAssemblies);

app.MapControllers();
// Add additional endpoints required by the Identity /Account Razor components.
app.MapIdentityApi<ApplicationUser>();
app.MapAdditionalIdentityEndpoints();
app.MapHub<UserMessageNotificationHub>(Constants.Routes.SignalRHubs.UserMessageHub);
app.MapGet("/api/blogpost/{blogPostId}/thumbnail", async (
    [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
    [FromRoute] long blogPostId,
    CancellationToken cancellationToken) =>
{
    var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
    var result = await dbContext.BlogPost
    .Where(p => p.BlogPostId == blogPostId)
    .Select(p => p.ThumbnailPhoto.PhotoBytes)
    .SingleAsync(cancellationToken);
    return TypedResults.File(result, System.Net.Mime.MediaTypeNames.Image.Jpeg);
});

app.UseSwagger();
app.UseSwaggerUI();

await app.RunAsync();

static void ConfigureCustomValidationAttributes(IServiceProvider services)
{
    //Find a way to use Source Generators for this in order to avoid using reflection
    var modelsAssembly = typeof(CustomRequiredAttribute).Assembly;
    var typesWithLocalizerAttribute =
        modelsAssembly.GetTypes()
        .Where(p =>
        p.CustomAttributes
        .Any(x => x.AttributeType.Name.Contains("LocalizerOfTAttribute")))
        .ToList();
    var localizerFactory = services.GetRequiredService<IStringLocalizerFactory>();
    foreach (var singleLocalizerType in typesWithLocalizerAttribute)
    {
        var newLocalizerInstance = localizerFactory.Create(singleLocalizerType);
        var field = singleLocalizerType
            .GetProperty("Localizer", BindingFlags.Public | BindingFlags.Static);
        field!.SetValue(null, newLocalizerInstance);
    }
}
static void ConfigureModelsLocalizers(IServiceProvider services)
{
    //Find a way to use Source Generators for this in order to avoid using reflection
    var modelsAssembly = typeof(FairPlayCombined.Models.UserModel).Assembly;
    var typesWithLocalizerAttribute =
        modelsAssembly.GetTypes()
        .Where(p =>
        p.CustomAttributes
        .Any(x => x.AttributeType.Name.Contains("LocalizerOfTAttribute")))
        .ToList();
    var localizerFactory = services.GetRequiredService<IStringLocalizerFactory>();
    foreach (var singleLocalizerType in typesWithLocalizerAttribute)
    {
        var newLocalizerInstance = localizerFactory.Create(singleLocalizerType);
        var field = singleLocalizerType
            .GetProperty("Localizer", BindingFlags.Public | BindingFlags.Static);
        field!.SetValue(null, newLocalizerInstance);
    }
}

static void AddPlatformServices(WebApplicationBuilder builder)
{
    builder.Services.AddTransient<ICustomCache, CustomCache>();
    builder.Services.AddTransient<ICultureService, CultureService>();
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
    builder.Services.AddTransient<IUserValidationService, UserValidationService>();
    builder.Services.AddTransient<IBlogService, BlogService>();
    builder.Services.AddTransient<IPhotoService, PhotoService>();
    builder.Services.AddTransient<IBlogPostService, BlogPostService>();
}