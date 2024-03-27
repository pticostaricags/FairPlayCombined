using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.Religion
{
    public class UpdateReligionModel : IUpdateModel
    {
        [Required]
        public int? ReligionId { get; set; }

        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
