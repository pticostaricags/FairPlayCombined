using FairPlayCombined.Models.AzureContentSafety;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IAzureContentSafetyService
    {
        Task<ImageModerationResultModel> AnalyzeImageAsync(byte[] imageBytes,
            CancellationToken cancellationToken);
        Task<TextModerationResultModel> AnalyzeTextAsync(string text, CancellationToken cancellationToken);
        Task<PromptShieldResponseModel> DetectJailbreakAttackAsync(PromptShieldRequestModel promptShieldRequestModel,
            CancellationToken cancellationToken);
    }
}
