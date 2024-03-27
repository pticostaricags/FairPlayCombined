using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.KidStatus
{
    public class UpdateKidStatusModel : IUpdateModel
    {
        [Required]
        public int? KidStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
