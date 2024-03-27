using Azure.AI.ContentSafety;

namespace FairPlayCombined.Services.Common
{
    public class AzureContentSafetyService(ContentSafetyClient contentSafetyClient)
    {
        public async Task<ImageModerationResultModel> AnalyzeImageAsync(byte[] imageBytes,
            CancellationToken cancellationToken)
        {
            BinaryData binaryData = new(imageBytes);
            var response = await contentSafetyClient.AnalyzeImageAsync(binaryData, cancellationToken);
            ImageModerationResultModel result = new()
            {
                IsAdult = response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                ImageCategory.Sexual)?.Severity > 0,
                IsHate = response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                ImageCategory.Hate)?.Severity > 0,
                IsViolence = response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                ImageCategory.Violence)?.Severity > 0,
                IsSelfHarm = response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                ImageCategory.SelfHarm)?.Severity > 0

            };
            return result;
        }
        public async Task<TextModerationResultModel> AnalyzeTextAsync(string text, CancellationToken cancellationToken)
        {
            var response = await contentSafetyClient.AnalyzeTextAsync(text, cancellationToken);
            TextModerationResultModel result = new()
            {
                IsSexuallyExplicity = response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                TextCategory.Sexual)?.Severity > 4,
                IsSexuallySuggestive = response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                TextCategory.Sexual)?.Severity > 0,
                IsOffensive = response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                TextCategory.Hate)?.Severity > 0 ||
                response?.Value?.CategoriesAnalysis?.SingleOrDefault(p => p.Category ==
                TextCategory.Violence)?.Severity > 0,
            };
            return result;
        }
    }
}
