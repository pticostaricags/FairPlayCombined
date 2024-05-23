namespace FairPlayCombined.Models.AzureContentSafety
{
    public class ImageModerationResultModel
    {
        public bool IsAdult { get; set; }
        public bool IsHate { get; set; }
        public bool IsViolence { get; set; }
        public bool IsSelfHarm { get; set; }
    }
}