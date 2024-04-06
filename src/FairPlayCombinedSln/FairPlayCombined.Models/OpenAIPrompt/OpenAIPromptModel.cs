using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.OpenAIPrompt
{
    public class OpenAIPromptModel : IListModel
    {
        [DeniedValues(default(long))]
        public long OpenAipromptId { get; set; }

        [Required]
        public string? OriginalPrompt { get; set; }

        [Required]
        public string? RevisedPrompt { get; set; }

        [Required]
        [StringLength(50)]
        public string? Model { get; set; }

        [Required]
        public byte[]? GeneratedImageBytes { get; set; }

        public DateTimeOffset RowCreationDateTime { get; set; }
    }
}
