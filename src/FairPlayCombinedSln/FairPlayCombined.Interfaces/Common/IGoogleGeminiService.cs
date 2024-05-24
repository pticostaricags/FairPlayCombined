using FairPlayCombined.Models.GoogleGemini;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IGoogleGeminiService
    {
        Task<GenerateContentResponseModel?> GenerateContentAsync(
            GenerateContentRequestModel generateContentRequestModel, CancellationToken cancellationToken);
    }
}
