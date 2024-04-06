using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
