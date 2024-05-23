using FairPlayCombined.Models.AzureContentSafety;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
