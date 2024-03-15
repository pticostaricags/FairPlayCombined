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

var googleAuthClientId = builder.Configuration["GoogleAuthClientId"] ??
    throw new InvalidOperationException("'GoogleAuthClientId' not found");
var googleAuthClientSecret = builder.Configuration["GoogleAuthClientSecret"] ??
    throw new InvalidOperationException("'GoogleAuthClientSecret' not found");

var googleAuthClientSecretsFilePath = builder.Configuration["GoogleAuthClientSecretsFilePath"] ??
    throw new InvalidOperationException("'GoogleAuthClientSecretsFilePath' not found");

var openAIKey = builder.Configuration["OpenAIKey"] ??
    throw new InvalidOperationException("'OpenAIKey' not found");

var generateDall3ImageUrl = builder.Configuration["GenerateDall3ImageUrl"] ??
    throw new InvalidOperationException("'GenerateDall3ImageUrl' not found");

var openAIChatCompletionsUrl = builder.Configuration["OpenAIChatCompletionsUrl"] ??
    throw new InvalidOperationException("'OpenAIChatCompletionsUrl' not found");

IResourceBuilder<IResourceWithConnectionString> sqlResourceWithConnectionString = 
    builder.AddConnectionString("FairPlayCombinedDb");

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
        callback.EnvironmentVariables.Add("AzureVideoIndexerAccountId", azureVideoIndexerAccountId);
        callback.EnvironmentVariables.Add("AzureVideoIndexerLocation", azureVideoIndexerLocation);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceGroup", azureVideoIndexerResourceGroup);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceName", azureVideoIndexerResourceName);
        callback.EnvironmentVariables.Add("AzureVideoIndexerSubscriptionId", azureVideoIndexerSubscriptionId);
        callback.EnvironmentVariables.Add("GoogleAuthClientId", googleAuthClientId);
        callback.EnvironmentVariables.Add("GoogleAuthClientSecret", googleAuthClientSecret);
        callback.EnvironmentVariables.Add("GoogleAuthClientSecretsFilePath", googleAuthClientSecretsFilePath);
        callback.EnvironmentVariables.Add("OpenAIKey", openAIKey);
        callback.EnvironmentVariables.Add("GenerateDall3ImageUrl", generateDall3ImageUrl);
        callback.EnvironmentVariables.Add("OpenAIChatCompletionsUrl", openAIChatCompletionsUrl);
    })
    .WithReference(sqlResourceWithConnectionString);
    builder.AddProject<Projects.FairPlayTube_VideoIndexing>("fairplaytubevideoindexing")
        .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("AzureVideoIndexerAccountId", azureVideoIndexerAccountId);
        callback.EnvironmentVariables.Add("AzureVideoIndexerLocation", azureVideoIndexerLocation);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceGroup", azureVideoIndexerResourceGroup);
        callback.EnvironmentVariables.Add("AzureVideoIndexerResourceName", azureVideoIndexerResourceName);
        callback.EnvironmentVariables.Add("AzureVideoIndexerSubscriptionId", azureVideoIndexerSubscriptionId);
    })
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
        .WithEnvironment(callback =>
        {
            callback.EnvironmentVariables.Add("AzureOpenAIEndpoint", builder.Configuration["AzureOpenAIEndpoint"]!);
            callback.EnvironmentVariables.Add("AzureOpenAIKey", builder.Configuration["AzureOpenAIKey"]!);
        })
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