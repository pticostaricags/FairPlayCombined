using FairPlayCombinedSln.AppHost;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

//Check: https://learn.microsoft.com/en-us/dotnet/aspire/extensibility/custom-resources?tabs=windows
var mailDev = builder.AddMailDev("maildev");

var googleAuthClientId = builder.Configuration["GoogleAuthClientId"] ??
        throw new InvalidOperationException("'GoogleAuthClientId' not found");

var googleAuthProjectId = builder.Configuration["GoogleAuthProjectId"] ??
        throw new InvalidOperationException("'GoogleAuthProjectId' not found");

var googleAuthUri = builder.Configuration["GoogleAuthUri"] ??
        throw new InvalidOperationException("'GoogleAuthUri' not found");

var googleAuthTokenUri = builder.Configuration["GoogleAuthTokenUri"] ??
        throw new InvalidOperationException("'GoogleAuthTokenUri' not found");

var googleAuthProviderCertUri = builder.Configuration["GoogleAuthProviderCertUri"] ??
        throw new InvalidOperationException("'GoogleAuthProviderCertUri' not found");

var googleAuthClientSecret = builder.Configuration["GoogleAuthClientSecret"] ??
        throw new InvalidOperationException("'GoogleAuthClientSecret' not found");

var googleAuthRedirectUri = builder.Configuration["GoogleAuthRedirectUri"] ??
        throw new InvalidOperationException("'GoogleAuthRedirectUri' not found");

var paypalClientId = builder.Configuration["PayPal:ClientId"] ??
    throw new InvalidOperationException("'PayPal:ClientId' not found");

var paypalClientSecret = builder.Configuration["PayPal:ClientSecret"] ??
    throw new InvalidOperationException("'PayPal:ClientSecret' not found");

IResourceBuilder<IResourceWithConnectionString>? fairPlayDbResource;
if (Convert.ToBoolean(builder.Configuration["UseDatabaseContainer"]))
{
    fairPlayDbResource = builder.AddSqlServer("sqlserver")
    // Mount the init scripts directory into the container.
    .WithBindMount("./sqlserverconfig", "/usr/config")
    // Mount the SQL scripts directory into the container so that the init scripts run.
    .WithBindMount("../FairPlayAdminPortal/data/sqlserver", "/docker-entrypoint-initdb.d")
    // Run the custom entrypoint script on startup.
    .WithEntrypoint("/usr/config/entrypoint.sh")
    // Add the database to the application model so that it can be referenced by other resources.
    .AddDatabase("FairPlayCombinedDb");
}
else
{
    fairPlayDbResource = builder.AddConnectionString("FairPlayCombinedDb");
}


bool addFairPlayDating = Convert.ToBoolean(builder.Configuration["AddFairPlayDating"]);
if (addFairPlayDating)
{
    builder.AddProject<Projects.FairPlayDating>(ResourcesNames.FairPlayDating)
        .WithReference(fairPlayDbResource)
        .WithReference(mailDev);
}

if (Convert.ToBoolean(builder.Configuration["AddFairPlayDatingTestDataGenerator"]))
{
    AddTestDataGenerator(builder, fairPlayDbResource);
}

bool addFairPlayTube = Convert.ToBoolean(builder.Configuration["AddFairPlayTube"]);
if (addFairPlayTube)
{
    builder.AddProject<Projects.FairPlayTube>(ResourcesNames.FairPlayTube)
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("GoogleAuthClientId", googleAuthClientId);
        callback.EnvironmentVariables.Add("GoogleAuthProjectId", googleAuthProjectId);
        callback.EnvironmentVariables.Add("GoogleAuthUri", googleAuthUri);
        callback.EnvironmentVariables.Add("GoogleAuthTokenUri", googleAuthTokenUri);
        callback.EnvironmentVariables.Add("GoogleAuthProviderCertUri", googleAuthProviderCertUri);
        callback.EnvironmentVariables.Add("GoogleAuthClientSecret", googleAuthClientSecret);
        callback.EnvironmentVariables.Add("GoogleAuthRedirectUri", googleAuthRedirectUri);

        callback.EnvironmentVariables.Add("PayPal:ClientId", paypalClientId);
        callback.EnvironmentVariables.Add("PayPal:ClientSecret", paypalClientSecret);
    })
    .WithReference(fairPlayDbResource)
    .WithReference(mailDev);
    builder.AddProject<Projects.FairPlayTube_VideoIndexing>(ResourcesNames.FairPlayTubeVideoIndexing)
        .WithReference(fairPlayDbResource);
}

bool addFairPlayShop = Convert.ToBoolean(builder.Configuration["AddFairPlayShop"]);
if (addFairPlayShop)
{
    builder.AddProject<Projects.FairPlayShop>(ResourcesNames.FairPlayShop)
    .WithReference(fairPlayDbResource)
    .WithReference(mailDev);
}

bool addCitiesImporter = Convert.ToBoolean(builder.Configuration["AddCitiesImporter"]);
if (addCitiesImporter)
{
    builder.AddProject<Projects.FairPlayCombined_CitiesImporter>(ResourcesNames.CitiesImporter)
        .WithReference(fairPlayDbResource);
}

bool addFairPlatAdminPortal = Convert.ToBoolean(builder.Configuration["AddFairPlatAdminPortal"]);
if (addFairPlatAdminPortal)
{
    builder.AddProject<Projects.FairPlayAdminPortal>(ResourcesNames.FairPlayAdminPortal)
        .WithReference(fairPlayDbResource)
        .WithReference(mailDev);
}

bool addFairPlaySocial = Convert.ToBoolean(builder.Configuration["AddFairPlaySocial"]);
if (addFairPlaySocial)
{
    builder.AddProject<Projects.FairPlaySocial>(ResourcesNames.FairPlaySocial)
        .WithReference(fairPlayDbResource)
        .WithReference(mailDev);
    if (Convert.ToBoolean(builder.Configuration["AddFairPlaySocialTestDataGenerator"]))
    {
        builder.AddProject<Projects.FairPlaySocial_TestDataGenerator>(ResourcesNames.FairPlaySocialTestDataGenerator)
            .WithReference(fairPlayDbResource);
    }
}


bool addLocalizationGenerator = Convert.ToBoolean(builder.Configuration["AddLocalizationGenerator"]);
if (addLocalizationGenerator)
{
    builder.AddProject<Projects.FairPlayCombined_LocalizationGenerator>(ResourcesNames.FairPlayCombinedLocalizationGenerator)
        .WithReference(fairPlayDbResource);
}
bool addFairPlayBudget = Convert.ToBoolean(builder.Configuration["AddFairPlayBudget"]);
if (addFairPlayBudget)
{
    builder.AddProject<Projects.FairPlayBudget>(ResourcesNames.FairPlayBudget)
        .WithReference(fairPlayDbResource)
        .WithReference(mailDev);
}

await builder.Build().RunAsync();

static void AddTestDataGenerator(IDistributedApplicationBuilder builder,
    IResourceBuilder<IResourceWithConnectionString> sqlServerResource)
{
    var humansPhotosDirectory = builder.Configuration["HumansPhotosDirectory"];
    builder.AddProject<Projects.FairPlayDating_TestDataGenerator>(ResourcesNames.FairPlayDatingTestDataGenerator)
        .WithEnvironment(callback =>
        {
            if (!String.IsNullOrWhiteSpace(humansPhotosDirectory))
            {
                callback.EnvironmentVariables.Add("HumansPhotosDirectory", humansPhotosDirectory);
            }
        }).WithReference(sqlServerResource);
}