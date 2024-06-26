using Azure.AI.OpenAI;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureOpenAI;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using System.Text.Json;

namespace FairPlayCombined.Services.Common
{
    public class AzureOpenAIService(OpenAIClient openAIClient,
        AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration,
        ILogger<AzureOpenAIService> logger) : IAzureOpenAIService
    {
        public enum ArticleMood
        {
            Hilarious,
            Funny,
            Professional
        }
        public async Task<string?> GenerateLinkedInArticleFromVideoCaptionsAsync(string videoTitle,
            string videoCaptions, ArticleMood articleMood, CancellationToken cancellationToken)
        {
            string systemMessage = "You will take the role of an expert in LinkedIn SEO. " +
                    "I will give you the information of one of my videos, your job is to use that information to create a draft LinkedIn article." +
                    $"Article must have a {articleMood} mood";
            if (articleMood == ArticleMood.Hilarious)
            {
                systemMessage = $"{systemMessage}. Add 5 {articleMood} jokes around the article.";
            }
            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = azureOpenAIServiceConfiguration.DeploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage(systemMessage),
                    new ChatRequestUserMessage($"Video Title: {videoTitle}." +
                    $"Video Captions: {videoCaptions}")
                }
            };
            var response = await openAIClient.GetChatCompletionsAsync(
                chatCompletionsOptions, cancellationToken: cancellationToken);
            var contentResponse =
            response.Value.Choices[0].Message.Content;
            return contentResponse;
        }
        public async Task<TextModerationResponse?> ModerateTextContentAsync(string text, CancellationToken cancellationToken)
        {
            TextModerationRequest textModerationRequest = new()
            {
                TextToModerate = text
            };
            TextModerationResponse textModerationResponseSkeleton = new()
            {
                IsOffensive = true,
                IsSexuallyExplicit = true,
                IsSexuallySuggestive = true,
                OffensivePhrases = ["Offensive Phrase 1", "Offensive Phrase 2"],
                PersonalIdentifiableInformation = ["PII Phrase 1", "PII Phrase 2"],
                Profanity = ["Profanity Phrase 1", "Profanity Phrase 2"],
                SexuallyExplicitPhrases = ["Sexually Explicit Phrase 1", "Sexually Explicit Phrase 2"],
                SexuallySuggestivePhrases = ["Sexually Suggestive Phrase 1", "Sexually Suggestive Phrase 2"],
                TextModerated = "Text moderated"
            };
            string jsonRequest = JsonSerializer.Serialize(textModerationRequest);
            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = azureOpenAIServiceConfiguration.DeploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage("You are an expert content moderator. " +
                    "Your jobs is to restrict text containing personal inormaton and any kind of gross content." +
                    "My requests will be in json format with the following properties:" +
                    $"{jsonRequest}" +
                    "Your responses must be in json format with the propertie I'll give you." +
                    "Avoid adding the ```json separators, give me only the json. Properties:" +
                    $"{JsonSerializer.Serialize(textModerationResponseSkeleton)}"),
                    new ChatRequestUserMessage(jsonRequest)
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
                    var filter = response.Value.PromptFilterResults[0];
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
                    int startOfError = ex.Message.IndexOf('{');
                    int endOfError = ex.Message.LastIndexOf('}');
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
                DeploymentName = azureOpenAIServiceConfiguration.DeploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage("You are an expert translator. Your jobs is to translate the text I give you." +
                    "My requests will be in json format with the following properties:" +
                    $"{nameof(TranslationRequest.OriginalText)}, {nameof(TranslationRequest.SourceLocale)}, {nameof(TranslationRequest.DestLocale)}" +
                    "Your responses must be in json format, in UTF-8, with the properties I'll give you. Return the json only, avoid adding the code block indicator ```json. JSON properties:" +
                    $"{nameof(TranslationResponse.SourceLocale)}, {nameof(TranslationResponse.DestLocale)}, {nameof(TranslationResponse.TranslatedText)}"),
                    new ChatRequestUserMessage(JsonSerializer.Serialize(translationRequest))
                }
            };
            var response = await openAIClient.GetChatCompletionsAsync(
                chatCompletionsOptions, cancellationToken: cancellationToken);
            var contentResponse =
            response.Value.Choices[0].Message.Content;
            logger.LogInformation("Content Response: {ContentResponse}", contentResponse);
            TranslationResponse? translationResponse =
                JsonSerializer.Deserialize<TranslationResponse>(contentResponse);
            return translationResponse;
        }

        public async Task<TranslationResponse[]?> TranslateMultipleTextsAsync(
            TranslationRequest[] textsToTranslate, CancellationToken cancellationToken)
        {
            ChatCompletionsOptions chatCompletionsOptions = new()
            {
                DeploymentName = azureOpenAIServiceConfiguration.DeploymentName,
                Messages =
                {
                    new ChatRequestSystemMessage("You are an expert translator. Your jobs is to translate the text I give you." +
                    "My requests will be in json format with the following properties:" +
                    $"{nameof(TranslationRequest.OriginalText)}, {nameof(TranslationRequest.SourceLocale)}, {nameof(TranslationRequest.DestLocale)}" +
                    "Your responses must be in json format with the following properties:" +
                    $"{nameof(TranslationResponse.OriginalText)}, {nameof(TranslationResponse.SourceLocale)}, {nameof(TranslationResponse.DestLocale)}, {nameof(TranslationResponse.TranslatedText)}"),
                    new ChatRequestUserMessage(JsonSerializer.Serialize(textsToTranslate))
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



#pragma warning disable S2166 // Classes named like "Exception" should extend "Exception" or a subclass
#pragma warning disable S101 // Types should be named in PascalCase
#pragma warning disable IDE1006 // Naming Styles
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
#pragma warning restore IDE1006 // Naming Styles