using Azure.AI.OpenAI;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureOpenAI;
using System.Text.Json;

namespace FairPlayCombined.Services.Common
{
    public class AzureOpenAIService(OpenAIClient openAIClient) : IAzureOpenAIService
    {
        public async Task<TranslationResponse?> TranslateSimpleTextAsync(string textToTranslate, string sourceLocale, string destLocale,
            CancellationToken cancellationToken)
        {
            TranslationRequest translationRequest = new()
            {
                OriginalText = textToTranslate,
                SourceLocale = sourceLocale,
                DestLocale = destLocale
            };
            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = "translationschat",
                Messages =
                {
                    new ChatMessage(ChatRole.System, "You are an expert translator. Your jobs is to translate the text I give you." +
                    "My requests will be in json format with the following properties:" +
                    $"{nameof(TranslationRequest.OriginalText)}, {nameof(TranslationRequest.SourceLocale)}, {nameof(TranslationRequest.DestLocale)}" +
                    "Your responses must be in json format with the following properties:" +
                    $"{nameof(TranslationResponse.SourceLocale)}, {nameof(TranslationResponse.DestLocale)}, {nameof(TranslationResponse.TranslatedText)}"),
                    new ChatMessage(ChatRole.User, JsonSerializer.Serialize(translationRequest))
                }
            };
            var response = await openAIClient.GetChatCompletionsAsync(
                chatCompletionsOptions, cancellationToken: cancellationToken);
            var contentResponse =
            response.Value.Choices[0].Message.Content;
            TranslationResponse? translationResponse =
                JsonSerializer.Deserialize<TranslationResponse>(contentResponse);
            return translationResponse;
        }

        public async Task<TranslationResponse[]?> TranslateMultipleTextsAsync(
            TranslationRequest[] textsToTranslate, CancellationToken cancellationToken)
        {
            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = "translationschat",
                Messages =
                {
                    new ChatMessage(ChatRole.System, "You are an expert translator. Your jobs is to translate the text I give you." +
                    "My requests will be in json format with the following properties:" +
                    $"{nameof(TranslationRequest.OriginalText)}, {nameof(TranslationRequest.SourceLocale)}, {nameof(TranslationRequest.DestLocale)}" +
                    "Your responses must be in json format with the following properties:" +
                    $"{nameof(TranslationResponse.OriginalText)}, {nameof(TranslationResponse.SourceLocale)}, {nameof(TranslationResponse.DestLocale)}, {nameof(TranslationResponse.TranslatedText)}"),
                    new ChatMessage(ChatRole.User, JsonSerializer.Serialize(textsToTranslate))
                }
            };
            var response = await openAIClient.GetChatCompletionsAsync(
                chatCompletionsOptions, cancellationToken: cancellationToken);
            var contentResponse =
            response.Value.Choices[0].Message.Content;
            TranslationResponse[]? translationResponse =
                JsonSerializer.Deserialize<TranslationResponse[]>(contentResponse);
            return translationResponse;
        }
    }

}
