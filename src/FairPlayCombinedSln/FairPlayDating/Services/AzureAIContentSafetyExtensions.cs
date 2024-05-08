using Azure.AI.ContentSafety;
using Azure;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Common;

namespace FairPlayDating.Extensions
{
    public static class AzureAIContentSafetyExtensions
    {
        public static void AddAzureAIContentSafety(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ContentSafetyClient>(sp =>
            {
                var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();

                var azureContentSafetyEndpoint = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_ENDPOINT_KEY} in database");

                var azureContentSafetyKey = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.AZURE_CONTENT_SAFETY_KEY_KEY} in database");


                ContentSafetyClient contentSafetyClient = new(new Uri(azureContentSafetyEndpoint.Value),
                            new AzureKeyCredential(azureContentSafetyKey.Value));
                return contentSafetyClient;
            });
            builder.Services.AddTransient<AzureContentSafetyService>(sp =>
            {
                ContentSafetyClient contentSafetyClient = sp.GetRequiredService<ContentSafetyClient>();
                AzureContentSafetyService azureContentSafetyService = new(contentSafetyClient);
                return azureContentSafetyService;
            });
        }
    }
}
