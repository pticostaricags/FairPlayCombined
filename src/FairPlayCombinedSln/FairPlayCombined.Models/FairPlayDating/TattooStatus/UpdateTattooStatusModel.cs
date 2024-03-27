using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.TattooStatus
{
    public class UpdateTattooStatusModel : IUpdateModel
    {
        [Required]
        public int? TattooStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
