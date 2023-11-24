using FairPlayCombined.CitiesImporter;
using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var fairPlayCombinedDbCS = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");

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

if (Convert.ToBoolean(builder.Configuration["AddFairPlayDatingTestDataGenerator"]))
{
    AddTestDataGenerator(builder, fairPlayCombinedDbCS);
}

if (Convert.ToBoolean(builder.Configuration["AddFairPlaySocialTestDataGenerator"]))
{
    builder.AddProject<Projects.FairPlaySocial_TestDataGenerator>("fairplaysocialtestdatagenerator")
        .WithEnvironment(callback =>
        {
            callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
        });
}

builder.AddProject<Projects.FairPlayCombined_LocalizationGenerator>("fairplaycombinedlocalizationgenerator")
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("AzureOpenAIEndpoint", builder.Configuration["AzureOpenAIEndpoint"]!);
        callback.EnvironmentVariables.Add("AzureOpenAIKey", builder.Configuration["AzureOpenAIKey"]!);
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });

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