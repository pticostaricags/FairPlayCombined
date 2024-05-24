using Azure.AI.ContentSafety;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.AzureContentSafety;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class AzureContentSafetyService(ContentSafetyClient contentSafetyClient,
        HttpClient authorizedHttpClient,
        AzureContentSafetyConfiguration azureContentSafetyConfiguration) : IAzureContentSafetyService
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

        public async Task<PromptShieldResponseModel> DetectJailbreakAttackAsync(PromptShieldRequestModel promptShieldRequestModel, CancellationToken cancellationToken)
        {
            var requestUrl = $"/contentsafety/text:shieldPrompt" +
                $"?api-version={azureContentSafetyConfiguration.ApiVersion}";
            var response = await authorizedHttpClient
                .PostAsJsonAsync(requestUrl, promptShieldRequestModel, cancellationToken: cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PromptShieldResponseModel>(cancellationToken);
            return result!;
        }
    }
}
