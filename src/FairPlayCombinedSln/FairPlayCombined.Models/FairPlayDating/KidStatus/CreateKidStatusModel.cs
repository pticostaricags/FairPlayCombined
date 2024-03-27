using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.KidStatus
{
    public class CreateKidStatusModel : ICreateModel
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
