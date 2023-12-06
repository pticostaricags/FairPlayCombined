using FairPlayCombined.CitiesImporter;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var azureOpenAIKey = builder.Configuration["AzureOpenAIKey"] ??
    throw new InvalidOperationException("'AzureOpenAIKey' not found");
var azureOpenAIEndpoint = builder.Configuration["AzureOpenAIEndpoint"] ??
    throw new InvalidOperationException("'AzureOpenAIEndpoint' not found");

var azureVideoIndexerAccountId = builder.Configuration["AzureVideoIndexerAccountId"] ??
    throw new InvalidOperationException("'AzureVideoIndexerAccountId' not found");
var azureVideoIndexerLocation = builder.Configuration["AzureVideoIndexerLocation"] ??
    throw new InvalidOperationException("'AzureVideoIndexerLocation' not found");
var azureVideoIndexerResourceGroup = builder.Configuration["AzureVideoIndexerResourceGroup"] ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceGroup' not found");
var azureVideoIndexerResourceName = builder.Configuration["AzureVideoIndexerResourceName"] ??
    throw new InvalidOperationException("'AzureVideoIndexerResourceName' not found");
var azureVideoIndexerSubscriptionId = builder.Configuration["AzureVideoIndexerSubscriptionId"] ??
    throw new InvalidOperationException("'AzureVideoIndexerSubscriptionId' not found");
var azureContentModeratorEndpoint = builder.Configuration["AzureContentModeratorEndpoint"] ??
    throw new InvalidOperationException("'AzureContentModeratorEndpoint' not found");
var azureContentModeratorKey = builder.Configuration["AzureContentModeratorKey"] ??
    throw new InvalidOperationException("'AzureContentModeratorKey' not found");

var sqlServerDbResource = builder.AddSqlServerContainer("FairPlayCombinedDbServer",
    password: "FairPlayCombinedDb$123$")
    .AddDatabase("FairPlayCombinedDb");

builder.AddProject<Projects.FairPlayCombined_DatabaseManager>("databasemanager")
    .WithReference(sqlServerDbResource);

bool addFairPlayDating = Convert.ToBoolean(builder.Configuration["AddFairPlayDating"]);
if (addFairPlayDating)
{
    builder.AddProject<Projects.FairPlayDating>(nameof(Projects.FairPlayDating).ToLower())
        .WithEnvironment(callback =>
        {
            callback.EnvironmentVariables.Add("AzureOpenAIKey", azureOpenAIKey);
            callback.EnvironmentVariables.Add("AzureOpenAIEndpoint", azureOpenAIEndpoint);
            callback.EnvironmentVariables.Add("AzureContentModeratorEndpoint", azureContentModeratorEndpoint);
            callback.EnvironmentVariables.Add("AzureContentModeratorKey", azureContentModeratorKey);
        })
        .WithReference(sqlServerDbResource);
    if (Convert.ToBoolean(builder.Configuration["AddFairPlayDatingTestDataGenerator"]))
    {
        AddTestDataGenerator(builder, sqlServerDbResource);
    }
}

bool addFairPlayTube = Convert.ToBoolean(builder.Configuration["AddFairPlayTube"]);
if (addFairPlayTube)
{
    builder.AddProject<Projects.FairPlayTube>(nameof(Projects.FairPlayTube).ToLower())
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("AzureVideoIndexerAccountId", azureVideoIndexerAccountId);
        callback.EnvironmentVariables.Add("AzureVideoIndexerLocation", azureVideoIndexerLocation);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceGroup", azureVideoIndexerResourceGroup);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceName", azureVideoIndexerResourceName);
        callback.EnvironmentVariables.Add("AzureVideoIndexerSubscriptionId", azureVideoIndexerSubscriptionId);

    })
    .WithReference(sqlServerDbResource);
    builder.AddProject<Projects.FairPlayTube_VideoIndexing>("fairplaytubevideoindexing")
        .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("AzureVideoIndexerAccountId", azureVideoIndexerAccountId);
        callback.EnvironmentVariables.Add("AzureVideoIndexerLocation", azureVideoIndexerLocation);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceGroup", azureVideoIndexerResourceGroup);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceName", azureVideoIndexerResourceName);
        callback.EnvironmentVariables.Add("AzureVideoIndexerSubscriptionId", azureVideoIndexerSubscriptionId);

    })
        .WithReference(sqlServerDbResource);
}

bool addFairPlayShop = Convert.ToBoolean(builder.Configuration["AddFairPlayShop"]);
if (addFairPlayShop)
{
    builder.AddProject<Projects.FairPlayShop>(nameof(Projects.FairPlayShop).ToLower())
    .WithReference(sqlServerDbResource);
}

bool addCitiesImporter = Convert.ToBoolean(builder.Configuration["AddCitiesImporter"]);
if (addCitiesImporter)
{
    builder.AddProject<Projects.FairPlayCombined_CitiesImporter>(nameof(CitiesImporter).ToLower())
        .WithReference(sqlServerDbResource);
}

bool addFairPlatAdminPortal = Convert.ToBoolean(builder.Configuration["AddFairPlatAdminPortal"]);
if (addFairPlatAdminPortal)
{
    builder.AddProject<Projects.FairPlayAdminPortal>(nameof(Projects.FairPlayAdminPortal).ToLower())
        .WithReference(sqlServerDbResource);
}

bool addFairPlaySocial = Convert.ToBoolean(builder.Configuration["AddFairPlaySocial"]);
if (addFairPlaySocial)
{
    builder.AddProject<Projects.FairPlaySocial>(nameof(Projects.FairPlaySocial).ToLower())
        .WithReference(sqlServerDbResource);
    if (Convert.ToBoolean(builder.Configuration["AddFairPlaySocialTestDataGenerator"]))
    {
        builder.AddProject<Projects.FairPlaySocial_TestDataGenerator>("fairplaysocialtestdatagenerator")
            .WithReference(sqlServerDbResource);
    }
}


bool addLocalizationGenerator = Convert.ToBoolean(builder.Configuration["AddLocalizationGenerator"]);
if (addLocalizationGenerator)
{
    builder.AddProject<Projects.FairPlayCombined_LocalizationGenerator>("fairplaycombinedlocalizationgenerator")
        .WithEnvironment(callback =>
        {
            callback.EnvironmentVariables.Add("AzureOpenAIEndpoint", builder.Configuration["AzureOpenAIEndpoint"]!);
            callback.EnvironmentVariables.Add("AzureOpenAIKey", builder.Configuration["AzureOpenAIKey"]!);
        })
        .WithReference(sqlServerDbResource);
}

builder.Build().Run();

static void AddTestDataGenerator(IDistributedApplicationBuilder builder,
    IResourceBuilder<SqlServerDatabaseResource> sqlServerDbResource)
{
    var humansPhotosDirectory = builder.Configuration["HumansPhotosDirectory"];
    builder.AddProject<Projects.FairPlayDating_TestDataGenerator>("fairplaydatingtestdatagenerator")
        .WithEnvironment(callback =>
        {
            if (!String.IsNullOrWhiteSpace(humansPhotosDirectory))
            {
                callback.EnvironmentVariables.Add("HumansPhotosDirectory", humansPhotosDirectory);
            }
        }).WithReference(sqlServerDbResource);
}