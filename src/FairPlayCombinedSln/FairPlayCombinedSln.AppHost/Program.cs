using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var fairPlayCombinedDbCS = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
builder.AddProject<Projects.FairPlayDating>("fairplaydating")
    .WithEnvironment(callback => 
    {
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });

builder.AddProject<Projects.FairPlayTube>("fairplaytube")
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });

builder.AddProject<Projects.FairPlayShop>("fairplayshop")
    .WithEnvironment(callback =>
    {
        callback.EnvironmentVariables.Add("FairPlayCombinedDb", fairPlayCombinedDbCS);
    });

builder.Build().Run();
