namespace FairPlayCombined.Services.Common
{
    public class ImageModerationResultModel
    {
        public bool IsAdult { get; internal set; }
        public bool IsHate { get; internal set; }
        public bool IsViolence { get; internal set; }
        public bool IsSelfHarm { get; internal set; }
    }
}