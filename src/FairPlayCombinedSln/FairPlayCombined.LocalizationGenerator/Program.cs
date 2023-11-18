using Azure.AI.OpenAI;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.LocalizationGenerator;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var connectionString = Environment.GetEnvironmentVariable("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(
    optionsAction =>
    {
        optionsAction.UseSqlServer(connectionString,
            sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
            });
    });
var azureOpenAIEndpoint =
            Environment.GetEnvironmentVariable("AzureOpenAIEndpoint") ??
            throw new ConfigurationException("Can't find config for AzureOpenAI:Endpoint");
var azureOpenAIKey = Environment.GetEnvironmentVariable("AzureOpenAIKey") ??
    throw new ConfigurationException("Can't find config for AzureOpenAI:Key");
builder.Services.AddTransient<OpenAIClient>(sp =>
{
    OpenAIClient openAIClient = new(endpoint: new Uri(azureOpenAIEndpoint),
        keyCredential: new Azure.AzureKeyCredential(azureOpenAIKey));
    return openAIClient;
});
builder.Services.AddTransient<IAzureOpenAIService, AzureOpenAIService>();
builder.Services.AddHostedService<LocalizationGenerator>();

var host = builder.Build();
host.Run();
