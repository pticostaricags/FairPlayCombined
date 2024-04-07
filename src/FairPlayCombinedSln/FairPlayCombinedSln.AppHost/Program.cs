using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);
builder.Configuration.AddUserSecrets<Program>();
string googleAuthClientInfo = string.Empty;
try
{
    var googleAuthClientSecretsFilePath = builder.Configuration["GoogleAuthClientSecretsFilePath"] ??
        throw new InvalidOperationException("'GoogleAuthClientSecretsFilePath' not found");
    googleAuthClientInfo = System.IO.File.ReadAllText(googleAuthClientSecretsFilePath);
}
catch (Exception)
{
    googleAuthClientInfo = builder.Configuration["GoogleAuthClientSecretsFileInfo"] ??
        throw new InvalidOperationException("'GoogleAuthClientSecretsFileInfo' not found");
}

IResourceBuilder<IResourceWithConnectionString> sqlResourceWithConnectionString =
    builder.AddConnectionString("FairPlayCombinedDb");

bool addFairPlayDating = Convert.ToBoolean(builder.Configuration["AddFairPlayDating"]);
if (addFairPlayDating)
{
    builder.AddProject<Projects.FairPlayDating>(nameof(Projects.FairPlayDating).ToLower())
        .WithReference(sqlResourceWithConnectionString);
    if (Convert.ToBoolean(builder.Configuration["AddFairPlayDatingTestDataGenerator"]))
    {
        AddTestDataGenerator(builder, sqlResourceWithConnectionString);
    }
}

bool addFairPlayTube = Convert.ToBoolean(builder.Configuration["AddFairPlayTube"]);
if (addFairPlayTube)
{
    builder.AddProject<Projects.FairPlayTube>(nameof(Projects.FairPlayTube).ToLower())
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("GoogleAuthClientInfo", googleAuthClientInfo);
    })
    .WithReference(sqlResourceWithConnectionString);
    builder.AddProject<Projects.FairPlayTube_VideoIndexing>("fairplaytubevideoindexing")
        .WithReference(sqlResourceWithConnectionString);
}

bool addFairPlayShop = Convert.ToBoolean(builder.Configuration["AddFairPlayShop"]);
if (addFairPlayShop)
{
    builder.AddProject<Projects.FairPlayShop>(nameof(Projects.FairPlayShop).ToLower())
    .WithReference(sqlResourceWithConnectionString);
}

bool addCitiesImporter = Convert.ToBoolean(builder.Configuration["AddCitiesImporter"]);
if (addCitiesImporter)
{
    builder.AddProject<Projects.FairPlayCombined_CitiesImporter>("citiesimporter")
        .WithReference(sqlResourceWithConnectionString);
}

bool addFairPlatAdminPortal = Convert.ToBoolean(builder.Configuration["AddFairPlatAdminPortal"]);
if (addFairPlatAdminPortal)
{
    builder.AddProject<Projects.FairPlayAdminPortal>(nameof(Projects.FairPlayAdminPortal).ToLower())
        .WithReference(sqlResourceWithConnectionString);
}

bool addFairPlaySocial = Convert.ToBoolean(builder.Configuration["AddFairPlaySocial"]);
if (addFairPlaySocial)
{
    builder.AddProject<Projects.FairPlaySocial>(nameof(Projects.FairPlaySocial).ToLower())
        .WithReference(sqlResourceWithConnectionString);
    if (Convert.ToBoolean(builder.Configuration["AddFairPlaySocialTestDataGenerator"]))
    {
        builder.AddProject<Projects.FairPlaySocial_TestDataGenerator>("fairplaysocialtestdatagenerator")
            .WithReference(sqlResourceWithConnectionString);
    }
}


bool addLocalizationGenerator = Convert.ToBoolean(builder.Configuration["AddLocalizationGenerator"]);
if (addLocalizationGenerator)
{
    builder.AddProject<Projects.FairPlayCombined_LocalizationGenerator>("fairplaycombinedlocalizationgenerator")
        .WithReference(sqlResourceWithConnectionString);
}
bool addFairPlayBudget = Convert.ToBoolean(builder.Configuration["AddFairPlayBudget"]);
if (addFairPlayBudget)
{
    builder.AddProject<Projects.FairPlayBudget>("fairplaybudget")
        .WithReference(sqlResourceWithConnectionString);
}

builder.Build().Run();

static void AddTestDataGenerator(IDistributedApplicationBuilder builder,
    IResourceBuilder<IResourceWithConnectionString> sqlServerResource)
{
    var humansPhotosDirectory = builder.Configuration["HumansPhotosDirectory"];
    builder.AddProject<Projects.FairPlayDating_TestDataGenerator>("fairplaydatingtestdatagenerator")
        .WithEnvironment(callback =>
        {
            if (!String.IsNullOrWhiteSpace(humansPhotosDirectory))
            {
                callback.EnvironmentVariables.Add("HumansPhotosDirectory", humansPhotosDirectory);
            }
        }).WithReference(sqlServerResource);
}