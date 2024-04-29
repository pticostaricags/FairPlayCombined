namespace FairPlayCombined.Services.Common
{
    public class TextModerationResultModel
    {
        public bool IsSexuallyExplicity { get; set; }
        public bool IsSexuallySuggestive { get; set; }
        public bool IsOffensive { get; set; }
    }
}