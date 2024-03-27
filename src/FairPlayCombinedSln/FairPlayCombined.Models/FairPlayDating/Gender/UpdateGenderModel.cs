using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.Gender
{
    public class UpdateGenderModel : IUpdateModel
    {
        [Required]
        public int? GenderId { get; set; }

        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
