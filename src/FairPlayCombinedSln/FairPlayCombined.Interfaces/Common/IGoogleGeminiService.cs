using FairPlayCombined.Models.GoogleGemini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IGoogleGeminiService
    {
        Task<GenerateContentResponseModel?> GenerateContentAsync(
            GenerateContentRequestModel generateContentRequestModel, CancellationToken cancellationToken);
    }
}
