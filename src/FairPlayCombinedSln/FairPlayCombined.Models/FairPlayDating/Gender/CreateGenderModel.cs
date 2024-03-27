using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.Gender
{
    public class CreateGenderModel : ICreateModel
    {
        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
