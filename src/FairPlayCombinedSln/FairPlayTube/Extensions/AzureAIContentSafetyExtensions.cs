using Azure.AI.ContentSafety;
using Azure;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Common;
using Microsoft.EntityFrameworkCore;
using FairPlayCombined.Models.AzureContentSafety;

namespace FairPlayTube.Extensions
{
    public static class AzureAIContentSafetyExtensions
    {
        public static void AddAzureAIContentSafety(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton(sp =>
            {
                var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
                var azureContentSafetyEndpoint = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY} in database");

                var azureContentSafetyKey = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY} in database");
                return new AzureContentSafetyConfiguration()
                {
                    Endpoint = azureContentSafetyEndpoint.Value,
                    Key = azureContentSafetyKey.Value,
                    ApiVersion = "2024-02-15-preview"
                };
            });
            builder.Services.AddTransient<ContentSafetyClient>(sp =>
            {
                var azureContentSafetyConfiguration = sp.GetRequiredService<AzureContentSafetyConfiguration>();

                ContentSafetyClient contentSafetyClient = new(
                    new Uri(azureContentSafetyConfiguration!.Endpoint!),
                            new AzureKeyCredential(azureContentSafetyConfiguration.Key!));
                return contentSafetyClient;
            });
            builder.Services.AddTransient<AzureContentSafetyService>(sp =>
            {
                AzureContentSafetyConfiguration azureContentSafetyConfiguration =
                sp.GetRequiredService<AzureContentSafetyConfiguration>();
                ContentSafetyClient contentSafetyClient = sp.GetRequiredService<ContentSafetyClient>();
                HttpClient authorizedHttpClient = new()
                {
                    BaseAddress = new Uri(azureContentSafetyConfiguration!.Endpoint!)
                };
                authorizedHttpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", azureContentSafetyConfiguration.Key);
                AzureContentSafetyService azureContentSafetyService =
                new(contentSafetyClient,
                authorizedHttpClient, azureContentSafetyConfiguration);
                return azureContentSafetyService;
            });
        }
    }
}
