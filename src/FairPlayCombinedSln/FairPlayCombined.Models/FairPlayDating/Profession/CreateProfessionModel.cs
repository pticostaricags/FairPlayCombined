using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.Profession
{
    public class CreateProfessionModel : ICreateModel
    {
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
    }
}
