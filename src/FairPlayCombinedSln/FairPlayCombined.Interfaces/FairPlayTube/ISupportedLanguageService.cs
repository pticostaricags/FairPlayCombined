using FairPlayCombined.Models.AzureVideoIndexer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface ISupportedLanguageService
    {
        Task<SupportedLanguageModel[]?> GetAllSupportedLanguageForVideoInfoIdAsync(long videoInfoId,
            CancellationToken cancellationToken);
        Task<SupportedLanguageModel[]?> GetAllSupportedLanguageAsync(CancellationToken cancellationToken);
    }
}
