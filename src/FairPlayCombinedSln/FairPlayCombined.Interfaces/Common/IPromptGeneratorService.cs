using FairPlayCombined.Models.Common.Promp;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IPromptGeneratorService
    {
        Task<PromptModel[]> GetAllPromptsAsync(CancellationToken cancellationToken);
        Task<bool> UpdateAllPromptsAsync(List<PromptModel> prompts, CancellationToken cancellationToken);
        Task<PromptModel?> GetPromptCompleteInfoAsync(string promptName,
            CancellationToken cancellationToken);
    }
}
