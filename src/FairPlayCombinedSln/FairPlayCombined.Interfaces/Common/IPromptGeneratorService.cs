using FairPlayCombined.Models.Common.Promp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
