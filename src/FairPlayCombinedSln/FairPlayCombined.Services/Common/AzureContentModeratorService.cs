using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Azure.CognitiveServices.ContentModerator.Models;
using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public class AzureContentModeratorService(ContentModeratorClient contentModeratorClient)
    {
        public async Task<ImageModerationResultModel> ModerateImageAsync(Stream imageStream, CancellationToken cancellationToken)
        {
            var evaluateResponse = await 
            contentModeratorClient.ImageModeration.EvaluateFileInputAsync(imageStream,
                cancellationToken: cancellationToken);
            ImageModerationResultModel result = new()
            {
                IsAdult = evaluateResponse.IsImageAdultClassified!.Value,
                IsRacy = evaluateResponse.IsImageRacyClassified!.Value
            };
            return result;
        }
        public async Task<TextModerationResultModel> ModeratePlainTextAsync(string text, CancellationToken cancellationToken)
        {
            var textBytes = Encoding.UTF8.GetBytes(text);
            MemoryStream stream = new MemoryStream(textBytes);
            var detectedLanguage = await contentModeratorClient.TextModeration
                .DetectLanguageAsync("text/plain", stream,
                cancellationToken: cancellationToken);
            stream = new MemoryStream(textBytes);
            var moderationResult = await contentModeratorClient.TextModeration
                .ScreenTextAsync(textContentType: "text/plain", textContent: stream,
                language: detectedLanguage.DetectedLanguageProperty,
                autocorrect:false, pII:true,classify:true, cancellationToken:cancellationToken);
            //Check https://learn.microsoft.com/en-us/azure/ai-services/content-moderator/text-moderation-api#classification
            TextModerationResultModel result = new TextModerationResultModel()
            {
                IsSexuallyExplicity = moderationResult.Classification.Category1.Score >= 0.5,
                IsSexuallySuggestive = moderationResult.Classification.Category2.Score >= 0.5,
                IsOffensive = moderationResult.Classification.Category3.Score >= 0.5,
                IsReviewRecommended = moderationResult.Classification.ReviewRecommended
            };
            return result;
        }
    }

    public class ImageModerationResultModel
    {
        public bool IsAdult { get; set; }
        public bool IsRacy { get; set; }
        public bool IsHate { get; set; }
        public bool IsViolence { get; set; }
        public bool IsSelfHarm { get; set; }
    }

    public class TextModerationResultModel
    {
        public bool IsSexuallyExplicity { get; internal set; }
        public bool IsSexuallySuggestive { get; internal set; }
        public bool IsOffensive { get; internal set; }
        public bool? IsReviewRecommended { get; internal set; }
    }
}
