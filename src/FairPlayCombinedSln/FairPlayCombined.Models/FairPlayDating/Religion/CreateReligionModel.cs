using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.Religion
{
    public class CreateReligionModel : ICreateModel
    {
        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
