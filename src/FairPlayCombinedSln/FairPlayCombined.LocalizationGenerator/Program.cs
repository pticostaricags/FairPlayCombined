using Azure.AI.OpenAI;
using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.LocalizationGenerator;
using FairPlayCombined.Models.AzureOpenAI;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();

var connectionString = builder.Configuration.GetConnectionString("FairPlayCombinedDb") ??
    throw new InvalidOperationException("Connection string 'FairPlayCombinedDb' not found.");
builder.Services.AddDbContextFactory<FairPlayCombinedDbContext>(
    optionsAction =>
    {
        optionsAction.UseSqlServer(connectionString,
            sqlServerOptionsAction =>
            {
                sqlServerOptionsAction.UseNetTopologySuite();
                sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(3),
                    errorNumbersToAdd: null);
            });
    });
builder.Services.AddSingleton<AzureOpenAIServiceConfiguration>(sp => 
{
    var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
    var dbContext = dbContextFactory.CreateDbContext();
    var azureOpenAIEndpointEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_OPENAI_ENDPOINT_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_OPENAI_ENDPOINT_KEY} in database");
    var azureOpenAIKeyEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_OPENAI_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_OPENAI_KEY_KEY} in database");
    var azureOpenAIDeploymentNameEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
    Constants.ConfigurationSecretsKeys.AZURE_OPENAI_DEPLOYMENT_NAME_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_OPENAI_DEPLOYMENT_NAME_KEY} in database");
    AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration = new()
    {
        DeploymentName = azureOpenAIDeploymentNameEntity.Value,
        Endpoint = azureOpenAIEndpointEntity.Value,
        Key = azureOpenAIKeyEntity.Value
    };
    return azureOpenAIServiceConfiguration;
});
builder.Services.AddTransient<OpenAIClient>(sp =>
{
    AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration =
    sp.GetRequiredService<AzureOpenAIServiceConfiguration>();
    OpenAIClient openAIClient = new(endpoint: new Uri(azureOpenAIServiceConfiguration.Endpoint!),
        keyCredential: new Azure.AzureKeyCredential(azureOpenAIServiceConfiguration.Key!));
    return openAIClient;
});
builder.Services.AddTransient<IAzureOpenAIService, AzureOpenAIService>();
builder.Services.AddHostedService<LocalizationGenerator>();

var host = builder.Build();
host.Run();
