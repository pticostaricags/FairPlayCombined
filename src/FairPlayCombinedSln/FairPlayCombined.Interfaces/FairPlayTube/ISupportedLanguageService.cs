using FairPlayCombined.Models.AzureVideoIndexer;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface ISupportedLanguageService
    {
        Task<SupportedLanguageModel[]?> GetAllSupportedLanguageForVideoInfoIdAsync(long videoInfoId,
            CancellationToken cancellationToken);
        Task<SupportedLanguageModel[]?> GetAllSupportedLanguageAsync(CancellationToken cancellationToken);
    }
}
