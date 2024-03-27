using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.HairColor
{
    public class UpdateHairColorModel : IUpdateModel
    {
        [Required]
        public int? HairColorId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
