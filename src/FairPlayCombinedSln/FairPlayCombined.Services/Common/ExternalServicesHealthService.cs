using Azure.AI.OpenAI;
using OpenAI.Chat;
using System.Runtime.Serialization;

namespace FairPlayCombined.Services.Common
{
    public class ExternalServicesHealthService(ExternalServicesConfigurationModel externalServicesConfigurationModel)
    {
        public async Task<ExternalServicesHealthModel[]> CheckServicesHealthAsync(
            CancellationToken cancellationToken)
        {
            ExternalServicesHealthModel azureOpenAIHealth = new();
            try
            {
                string systemMessage = "Reply with 'YES'";
                var chatClient = externalServicesConfigurationModel.OpenAIClient!.GetChatClient(externalServicesConfigurationModel.AzureOpenDeploymentName);
                var chatCompletion = await chatClient.CompleteChatAsync(messages:
                    new[]
                    {
                        new SystemChatMessage(systemMessage)
                    }, cancellationToken: cancellationToken);
                var contentResponse = chatCompletion.Value.Content[0].Text;
                azureOpenAIHealth.Response = contentResponse!.ToString();
                azureOpenAIHealth.IsHealthy = true;
            }
            catch (Exception ex)
            {
                azureOpenAIHealth.Response = ex.ToString();
                azureOpenAIHealth.IsHealthy = false;
            }
            return [azureOpenAIHealth];
        }
    }

    public class ExternalServicesConfigurationModel
    {
        public string? AzureOpenDeploymentName { get; set; }
        public Azure.AI.OpenAI.AzureOpenAIClient? OpenAIClient { get; set; }
    }

    public class ExternalServicesHealthModel
    {
        public string? ServiceName { get; set; }
        public bool IsHealthy { get; set; }
        public string? Response { get; set; }
    }
}
