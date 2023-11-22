using Azure.AI.OpenAI;
using Azure.Core.Pipeline;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureOpenAI;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FairPlayCombined.Services.Common
{
    public class AzureOpenAIService(OpenAIClient openAIClient) : IAzureOpenAIService
    {
        private const string DeploymentName = "translationschat";

        public async Task<TextModerationResponse?> ModerateTextContentAsync(string text, CancellationToken cancellationToken)
        {
            TextModerationRequest textModerationRequest = new TextModerationRequest()
            {
                TextToModerate = text
            };
            TextModerationResponse textModerationResponseSkeleton = new()
            {
                IsOffensive = true,
                IsSexuallyExplicit = true,
                IsSexuallySuggestive = true,
                OffensivePhrases = new[] { "Offensive Phrase 1", "Offensive Phrase 2" },
                PersonalIdentifiableInformation = new[] { "PII Phrase 1", "PII Phrase 2" },
                Profanity = new string[] { "Profanity Phrase 1", "Profanity Phrase 2" },
                SexuallyExplicitPhrases = new string[] { "Sexually Explicit Phrase 1", "Sexually Explicit Phrase 2" },
                SexuallySuggestivePhrases = new string[] { "Sexually Suggestive Phrase 1", "Sexually Suggestive Phrase 2" },
                TextModerated = "Text moderated"
            };
            string jsonRequest = JsonSerializer.Serialize(textModerationRequest);
            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = DeploymentName,
                Messages =
                {
                    new ChatMessage(ChatRole.System, "You are an expert content moderator. " +
                    "Your jobs is to restrict text containing personal inormaton and any kind of gross content." +
                    "My requests will be in json format with the following properties:" +
                    $"{jsonRequest}" +
                    "Your responses must be in json format with the following properties:" +
                    $"{JsonSerializer.Serialize(textModerationResponseSkeleton)}"),
                    new ChatMessage(ChatRole.User, jsonRequest)
                }
            };
            try
            {
                var response = await openAIClient.GetChatCompletionsAsync(
                    chatCompletionsOptions, cancellationToken: cancellationToken);
                var contentResponse =
                response.Value.Choices[0].Message.Content;
                if (contentResponse is null)
                {
                    var filter = response.Value.PromptFilterResults.First();
                    return new TextModerationResponse()
                    {
                        IsSexuallyExplicit = filter.ContentFilterResults.Sexual.Severity == ContentFilterSeverity.High,
                        IsSexuallySuggestive = filter.ContentFilterResults.Sexual.Severity != ContentFilterSeverity.High,
                        IsOffensive = filter.ContentFilterResults.Hate.Severity != ContentFilterSeverity.Safe
                    };
                }
                TextModerationResponse? textModerationResponse =
                    JsonSerializer.Deserialize<TextModerationResponse>(contentResponse);
                return textModerationResponse;
            }
            catch (Azure.RequestFailedException ex)
            {
                if (ex.ErrorCode == "content_filter")
                {
                    int startOfError = ex.Message.IndexOf("{");
                    int endOfError = ex.Message.LastIndexOf("}");
                    string errorContent = ex.Message.Substring(startOfError, endOfError - startOfError + 1);
                    ContentFilterJsonException contentFilterJsonException =
                        JsonSerializer.Deserialize<ContentFilterJsonException>(errorContent)!;
                    return new TextModerationResponse()
                    {
                        IsOffensive = contentFilterJsonException!.error!.innererror!.content_filter_result!.hate!.filtered,
                        IsSexuallyExplicit = contentFilterJsonException.error.innererror.content_filter_result.sexual!.filtered,
                        IsSexuallySuggestive = contentFilterJsonException.error.innererror.content_filter_result.sexual.filtered,
                    };
                }
                throw;
            }
        }
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
                DeploymentName = DeploymentName,
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
                DeploymentName = DeploymentName,
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



    public class ContentFilterJsonException
    {
        public Error? error { get; set; }
    }

    public class Error
    {
        public string? message { get; set; }
        public object? type { get; set; }
        public string? param { get; set; }
        public string? code { get; set; }
        public int status { get; set; }
        public Innererror? innererror { get; set; }
    }

    public class Innererror
    {
        public string? code { get; set; }
        public Content_Filter_Result? content_filter_result { get; set; }
    }

    public class Content_Filter_Result
    {
        public Hate? hate { get; set; }
        public Self_Harm? self_harm { get; set; }
        public Sexual? sexual { get; set; }
        public Violence? violence { get; set; }
    }

    public class Hate
    {
        public bool filtered { get; set; }
        public string? severity { get; set; }
    }

    public class Self_Harm
    {
        public bool filtered { get; set; }
        public string? severity { get; set; }
    }

    public class Sexual
    {
        public bool filtered { get; set; }
        public string? severity { get; set; }
    }

    public class Violence
    {
        public bool filtered { get; set; }
        public string? severity { get; set; }
    }


}
