using Azure.AI.OpenAI;

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
                ChatCompletionsOptions chatCompletionsOptions = new()
                {
                    DeploymentName = externalServicesConfigurationModel.AzureOpenDeploymentName,
                    Messages =
                {
                    new ChatRequestSystemMessage(systemMessage)
                }
                };
                var response = await externalServicesConfigurationModel.OpenAIClient!.GetChatCompletionsAsync(
                chatCompletionsOptions, cancellationToken: cancellationToken);
                var contentResponse =
                response.Value.Choices[0].Message.Content;
                azureOpenAIHealth.Response = contentResponse;
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
        public Azure.AI.OpenAI.OpenAIClient? OpenAIClient { get; set; }
    }

    public class ExternalServicesHealthModel
    {
        public string? ServiceName { get; set; }
        public bool IsHealthy { get; set; }
        public string? Response { get; set; }
    }
}
