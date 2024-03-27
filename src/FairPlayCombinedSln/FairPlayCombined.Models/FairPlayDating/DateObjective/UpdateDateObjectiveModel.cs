using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.DateObjective
{
    public class UpdateDateObjectiveModel : IUpdateModel
    {
        [Required]
        public int? DateObjectiveId { get; set; }

        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
