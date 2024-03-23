using Azure.AI.OpenAI;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using Microsoft.Azure.CognitiveServices.ContentModerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FairPlayCombined.Services.Common.AzureOpenAIService;

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
                    DeploymentName = AzureOpenAIService.DeploymentName,
                    Messages =
                {
                    new ChatMessage(ChatRole.System, systemMessage)
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
        public Azure.AI.OpenAI.OpenAIClient? OpenAIClient { get; set; }
        public ContentModeratorClient? ContentModeratorClient
        {
            get; set;
        }
    }

    public class ExternalServicesHealthModel
    {
        public string? ServiceName { get; set; }
        public bool IsHealthy { get; set; }
        public string? Response { get; set; }
    }
}
