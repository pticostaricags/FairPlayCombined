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


builder.Build().Run();
