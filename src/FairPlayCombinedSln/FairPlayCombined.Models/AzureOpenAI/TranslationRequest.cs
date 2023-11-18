namespace FairPlayCombined.Models.AzureOpenAI
{
    public class TranslationRequest
    {
        public string? OriginalText { get; set; }
        public string? SourceLocale { get; set; }
        public string? DestLocale { get; set; }
    }
}
