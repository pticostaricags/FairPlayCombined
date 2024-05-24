using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.OpenAIPrompt
{
    public class CreateOpenAIPromptModel : ICreateModel
    {

        [Required]
        public string? OriginalPrompt { get; set; }

        [Required]
        public string? RevisedPrompt { get; set; }

        [Required]
        [StringLength(50)]
        public string? Model { get; set; }

        [Required]
        public byte[]? GeneratedImageBytes { get; set; }
    }
}
