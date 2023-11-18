using FairPlayCombined.Models.AzureOpenAI;

namespace FairPlayCombined.Interfaces
{
    public interface IAzureOpenAIService
    {
        Task<TranslationResponse[]?> TranslateMultipleTextsAsync(TranslationRequest[] textsToTranslate, CancellationToken cancellationToken);
        Task<TranslationResponse?> TranslateSimpleTextAsync(string textToTranslate, string sourceLocale, string destLocale, CancellationToken cancellationToken);
    }
}
