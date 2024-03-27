using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.Frequency
{
    public class UpdateFrequencyModel : IUpdateModel
    {
        [Required]
        public int? FrequencyId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
