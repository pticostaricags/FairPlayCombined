using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.PromptVariable
{
    public class PromptVariableModel
    {
        [Required]
        [DeniedValues(default(int))]
        public int PromptId { get; set; }

        [Required]
        [StringLength(50)]
        public string? VariableName { get; set; }
    }
}
