using Azure.AI.OpenAI;
using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureOpenAI;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Services.Extensions
{
    public static class ServicesConfigurationExtensions
    {
        public static IServiceCollection AddAzureOpenAIService(this IServiceCollection services)
        {
            services.AddSingleton<AzureOpenAIServiceConfiguration>(sp =>
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

            services.AddTransient<AzureOpenAIClient>(sp =>
            {
                AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration =
                sp.GetRequiredService<AzureOpenAIServiceConfiguration>();
                AzureOpenAIClient openAIClient = new(endpoint: new Uri(azureOpenAIServiceConfiguration.Endpoint!),
                    credential: new Azure.AzureKeyCredential(azureOpenAIServiceConfiguration.Key!));
                return openAIClient;
            });
            services.AddTransient<IAzureOpenAIService, AzureOpenAIService>(sp =>
            {
                AzureOpenAIClient openAIClient = sp.GetRequiredService<AzureOpenAIClient>();
                AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration =
                sp.GetRequiredService<AzureOpenAIServiceConfiguration>();
                var logger = sp.GetRequiredService<ILogger<AzureOpenAIService>>();
                AzureOpenAIService azureOpenAIService = new(openAIClient, 
                    azureOpenAIServiceConfiguration, logger);
                return azureOpenAIService;
            });
            return services;
        }
    }
}
