using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.EyesColor
{
    public class CreateEyesColorModel : ICreateModel
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
