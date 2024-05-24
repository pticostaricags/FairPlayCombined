using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.NewVideoScript
{
    public class NewVideoScriptModel
    {
        [Required]
        public string? VideoTitle { get; set; }
        [Required]
        public string? VideoDescription { get; set; }
        [ValidateComplexType]
        public NewVideoScriptLink[]? Links { get; set; }
    }

    public class NewVideoScriptLink
    {
        [Required]
        [Url]
        public string? Url { get; set; }
    }
}
