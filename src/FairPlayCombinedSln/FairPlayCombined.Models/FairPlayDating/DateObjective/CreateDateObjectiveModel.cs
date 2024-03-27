using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.DateObjective
{
    public class CreateDateObjectiveModel : ICreateModel
    {
        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
