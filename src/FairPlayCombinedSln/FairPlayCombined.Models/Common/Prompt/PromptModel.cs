using FairPlayCombined.Models.Common.PromptVariable;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.Promp
{
    public class PromptModel
    {
        public int PromptId { get; set; }

        [Required]
        [StringLength(100)]
        public string? PromptName { get; set; }

        [Required]
        public string? BaseText { get; set; }
    }
}
