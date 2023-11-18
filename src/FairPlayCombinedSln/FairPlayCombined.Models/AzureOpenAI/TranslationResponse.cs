namespace FairPlayCombined.Models.AzureOpenAI
{
    public class TranslationResponse
    {
        public string? OriginalText { get; set; }
        public string? SourceLocale { get; set; }
        public string? DestLocale { get; set; }
        public string? TranslatedText { get; set; }
    }
}
