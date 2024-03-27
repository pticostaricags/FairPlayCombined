using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.TattooStatus
{
    public class CreateTattooStatusModel : ICreateModel
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
