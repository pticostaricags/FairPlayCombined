using FairPlayCombined.CitiesImporter;
using FairPlayCombined.Common.CustomExceptions;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var fairPlayCombinedDbCS = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
var azureOpenAIEndpoint = builder.Configuration["AzureOpenAIEndpoint"] ?? throw new ConfigurationException("Can't find config for AzureOpenAI:Endpoint");
var azureOpenAIKey =
    builder.Configuration["AzureOpenAIKey"] ?? throw new ConfigurationException("Can't find config for AzureOpenAI:Key");

builder.AddProject<Projects.FairPlayDating>(nameof(Projects.FairPlayDating).ToLower())
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });


builder.AddProject<Projects.FairPlayTube>(nameof(Projects.FairPlayTube).ToLower())
.WithEnvironment(callback =>
{
    callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
});


builder.AddProject<Projects.FairPlayShop>(nameof(Projects.FairPlayShop).ToLower())
.WithEnvironment(callback =>
{
    callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
});


builder.AddProject<Projects.FairPlayCombined_CitiesImporter>(nameof(CitiesImporter).ToLower())
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });

builder.AddProject<Projects.FairPlayAdminPortal>(nameof(Projects.FairPlayAdminPortal).ToLower())
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });


builder.AddProject<Projects.FairPlaySocial>(nameof(Projects.FairPlaySocial).ToLower())
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });

//AddTestDataGenerator(builder, fairPlayCombinedDbCS);

builder.Build().Run();

static void AddTestDataGenerator(IDistributedApplicationBuilder builder, string fairPlayCombinedDbCS)
{
    var humansPhotosDirectory = builder.Configuration["HumansPhotosDirectory"];
    builder.AddProject<Projects.FairPlayDating_TestDataGenerator>("fairplaydatingtestdatagenerator")
        .WithEnvironment(callback =>
        {
            callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
            if (!String.IsNullOrWhiteSpace(humansPhotosDirectory))
            {
                callback.EnvironmentVariables.Add("HumansPhotosDirectory", humansPhotosDirectory);
            }
        });
}