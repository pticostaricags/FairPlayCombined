using FairPlayCombinedSln.AppHost;
using FairPlayCombinedSln.AppHost.Extensions.AzureVideoIndexer;
using Microsoft.Extensions.Configuration;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();

bool useSendGrid = Convert.ToBoolean(builder.Configuration["UseSendGrid"]);
IResourceBuilder<MailDevResource>? mailDev = null;
if (!useSendGrid)
    mailDev = ConfigureMailDev(builder);

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

var paypalClientId = builder.Configuration["PayPalClientId"] ??
    throw new InvalidOperationException("'PayPalClientId' not found");

var paypalClientSecret = builder.Configuration["PayPalClientSecret"] ??
    throw new InvalidOperationException("'PayPalClientSecret' not found");

var ipDataKey = builder.Configuration["IpDataKey"] ??
    throw new InvalidOperationException("'IpDataKey' not found");

IResourceBuilder<IResourceWithConnectionString>? fairPlayDbResource = ConfigureDatabase(builder);

var blobs = builder
    .AddAzureStorage("storage")
    .AddBlobs("blobs");

bool addFairPlayDating = Convert.ToBoolean(builder.Configuration["AddFairPlayDating"]);
if (addFairPlayDating)
{
    var fairPlayDating =
    builder.AddProject<Projects.FairPlayDating>(ResourcesNames.FairPlayDating)
    .WithExternalHttpEndpoints()
    .WithReference(fairPlayDbResource);
    if (!useSendGrid)
        fairPlayDating = fairPlayDating.WithReference(mailDev!);
    else
    {
        fairPlayDating = fairPlayDating.WithEnvironment((Action<EnvironmentCallbackContext>)(callback =>
        {
            AddSMTPEnvironmentVariables(callback, builder);
        }));
    }
}

if (Convert.ToBoolean(builder.Configuration["AddFairPlayDatingTestDataGenerator"]))
{
    AddTestDataGenerator(builder, fairPlayDbResource);
}

bool addFairPlayTube = Convert.ToBoolean(builder.Configuration["AddFairPlayTube"]);
if (addFairPlayTube)
{
    var fairPlayTube =
    builder.AddProject<Projects.FairPlayTube>(ResourcesNames.FairPlayTube);

    if (builder.ExecutionContext.IsPublishMode)
    {
        fairPlayTube = fairPlayTube
            .WithExternalHttpEndpoints()
            .WithEndpoint(port: 19390, targetPort: 19390, scheme: "tcp");
    }
    fairPlayTube = fairPlayTube
        .WithEnvironment(callback =>
        {
            callback.EnvironmentVariables.Add("GoogleAuthClientId", googleAuthClientId);
            callback.EnvironmentVariables.Add("GoogleAuthProjectId", googleAuthProjectId);
            callback.EnvironmentVariables.Add("GoogleAuthUri", googleAuthUri);
            callback.EnvironmentVariables.Add("GoogleAuthTokenUri", googleAuthTokenUri);
            callback.EnvironmentVariables.Add("GoogleAuthProviderCertUri", googleAuthProviderCertUri);
            callback.EnvironmentVariables.Add("GoogleAuthClientSecret", googleAuthClientSecret);
            callback.EnvironmentVariables.Add("GoogleAuthRedirectUri", googleAuthRedirectUri);

            callback.EnvironmentVariables.Add("PayPalClientId", paypalClientId);
            callback.EnvironmentVariables.Add("PayPalClientSecret", paypalClientSecret);

            callback.EnvironmentVariables.Add("IpDataKey", ipDataKey);
        })
    .WithReference(fairPlayDbResource)
    .WithReference(blobs);

    if (!useSendGrid)
        fairPlayTube = fairPlayTube.WithReference(mailDev!);
    else
        fairPlayTube = fairPlayTube.WithEnvironment(callback =>
        {
            AddSMTPEnvironmentVariables(callback, builder);
        });
}

if (Convert.ToBoolean(builder.Configuration["AddFairPlayTubeVideoIndexing"]))
{
    builder.AddProject<Projects.FairPlayTube_VideoIndexing>(ResourcesNames.FairPlayTubeVideoIndexing)
        .WithReference(fairPlayDbResource);
}

bool addFairPlayShop = Convert.ToBoolean(builder.Configuration["AddFairPlayShop"]);
if (addFairPlayShop)
{
    var fairPlayShop =
    builder.AddProject<Projects.FairPlayShop>(ResourcesNames.FairPlayShop)
    .WithExternalHttpEndpoints()
    .WithReference(fairPlayDbResource);
    if (!useSendGrid)
        fairPlayShop = fairPlayShop.WithReference(mailDev!);
    else
    {
        fairPlayShop = fairPlayShop.WithEnvironment(callback =>
        {
            AddSMTPEnvironmentVariables(callback, builder);
        });
    }
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
    var fairPlatAdminPortal =
    builder.AddProject<Projects.FairPlayAdminPortal>(ResourcesNames.FairPlayAdminPortal)
    .WithExternalHttpEndpoints()
    .WithReference(fairPlayDbResource);
    if (!useSendGrid)
        fairPlatAdminPortal = fairPlatAdminPortal.WithReference(mailDev!);
    else
    {
        fairPlatAdminPortal = fairPlatAdminPortal.WithEnvironment(callback =>
        {
            AddSMTPEnvironmentVariables(callback, builder);
        });
    }
}

bool addFairPlaySocial = Convert.ToBoolean(builder.Configuration["AddFairPlaySocial"]);
if (addFairPlaySocial)
{
    var fairPlaySocial =
    builder.AddProject<Projects.FairPlaySocial>(ResourcesNames.FairPlaySocial)
    .WithExternalHttpEndpoints()
    .WithReference(fairPlayDbResource);
    if (!useSendGrid)
        fairPlaySocial = fairPlaySocial.WithReference(mailDev!);
    else
    {
        fairPlaySocial = fairPlaySocial.WithEnvironment(callback =>
        {
            AddSMTPEnvironmentVariables(callback, builder);
        });
    }
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
    var fairPlayBudget =
    builder.AddProject<Projects.FairPlayBudget>(ResourcesNames.FairPlayBudget)
    .WithExternalHttpEndpoints()
    .WithReference(fairPlayDbResource);
    if (!useSendGrid)
        fairPlayBudget = fairPlayBudget.WithReference(mailDev!);
    else
    {
        fairPlayBudget = fairPlayBudget.WithEnvironment(callback =>
        {
            AddSMTPEnvironmentVariables(callback, builder);
        });
    }
}

bool addFairPlayCRM = Convert.ToBoolean(builder.Configuration["AddFairPlayCRM"]);
if (addFairPlayCRM)
{
    var fairPlayCRM =
    builder.AddProject<Projects.FairPlayCRM>(ResourcesNames.FairPlayCRM);

    if (builder.ExecutionContext.IsPublishMode)
    {
        fairPlayCRM = fairPlayCRM
            .WithExternalHttpEndpoints()
            .WithEndpoint(port: 19390, targetPort: 19390, scheme: "tcp");
    }
    fairPlayCRM = fairPlayCRM
        .WithEnvironment(callback =>
        {
            callback.EnvironmentVariables.Add("GoogleAuthClientId", googleAuthClientId);
            callback.EnvironmentVariables.Add("GoogleAuthProjectId", googleAuthProjectId);
            callback.EnvironmentVariables.Add("GoogleAuthUri", googleAuthUri);
            callback.EnvironmentVariables.Add("GoogleAuthTokenUri", googleAuthTokenUri);
            callback.EnvironmentVariables.Add("GoogleAuthProviderCertUri", googleAuthProviderCertUri);
            callback.EnvironmentVariables.Add("GoogleAuthClientSecret", googleAuthClientSecret);
            callback.EnvironmentVariables.Add("GoogleAuthRedirectUri", googleAuthRedirectUri);

            callback.EnvironmentVariables.Add("PayPalClientId", paypalClientId);
            callback.EnvironmentVariables.Add("PayPalClientSecret", paypalClientSecret);

            callback.EnvironmentVariables.Add("IpDataKey", ipDataKey);
        })
    .WithReference(fairPlayDbResource)
    .WithReference(blobs);

    if (!useSendGrid)
        fairPlayCRM = fairPlayCRM.WithReference(mailDev!);
    else
        fairPlayCRM = fairPlayCRM.WithEnvironment(callback =>
        {
            AddSMTPEnvironmentVariables(callback, builder);
        });
}


builder.AddProject<Projects.FairPlayCombined_WebApi>(ResourcesNames.FairPlayWebApi)
.WithExternalHttpEndpoints()
.WithReference(fairPlayDbResource);

if (Convert.ToBoolean(builder.Configuration["DeployAzureVideoIndexer"]))
{
    builder.AddAzureVideoIndexer(name: "mainvideoindexer");
}

var dataExportService =
    builder.AddProject<Projects.FairPlayCombined_DataExportService>("dataexportservice")
    .WithReference(fairPlayDbResource)
    .WithReference(blobs);

if (!useSendGrid)
    dataExportService = dataExportService.WithReference(mailDev!);
else
{
    dataExportService = dataExportService.WithEnvironment(callback =>
    {
        AddSMTPEnvironmentVariables(callback, builder);
    });
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

static IResourceBuilder<MailDevResource>? ConfigureMailDev(IDistributedApplicationBuilder builder)
{
    //Check: https://learn.microsoft.com/en-us/dotnet/aspire/extensibility/custom-resources?tabs=windows
    return builder.AddMailDev("smtp");
}

static IResourceBuilder<IResourceWithConnectionString> ConfigureDatabase(IDistributedApplicationBuilder builder)
{
    IResourceBuilder<IResourceWithConnectionString>? fairPlayDbResource;
    if (Convert.ToBoolean(builder.Configuration["UseDatabaseContainer"]))
    {
        var sqlPassword = builder.AddParameter("db-password", secret: true);
        fairPlayDbResource = builder.AddSqlServer("dbserver", password: sqlPassword)
            .WithDataVolume()
            .AddDatabase("FairPlayCombinedDb");
    }
    else
    {
        fairPlayDbResource = builder.AddConnectionString("FairPlayCombinedDb");
    }

    builder.AddProject<Projects.FairPlayCombined_DatabaseManager>(ResourcesNames.DatabaseManager)
        .WithReference(fairPlayDbResource);
    return fairPlayDbResource;
}

static void AddSMTPEnvironmentVariables(EnvironmentCallbackContext callback, IDistributedApplicationBuilder builder)
{
    var smtpServer = builder.Configuration["SMTPServer"];
    var smtpPort = builder.Configuration["SMTPPort"];
    var smtpUsername = builder.Configuration["SMTPUsername"];
    var smtpPassword = builder.Configuration["SMTPPassword"];
    var useSSLForSMTP = builder.Configuration["UseSSLForSMTP"];
    var useSendGrid = builder.Configuration["UseSendGrid"];
    var emailFrom = builder.Configuration["EmailFrom"];
    callback.EnvironmentVariables.Add("EmailFrom", emailFrom!);
    callback.EnvironmentVariables.Add("UseSendGrid", useSendGrid!);
    callback.EnvironmentVariables.Add("UseSSLForSMTP", useSSLForSMTP!);
    callback.EnvironmentVariables.Add("SMTPUsername", smtpUsername!);
    callback.EnvironmentVariables.Add("SMTPPassword", smtpPassword!);
    callback.EnvironmentVariables.Add("ConnectionStrings__smtp",
        $"smtp://{smtpServer}:{smtpPort}");
}