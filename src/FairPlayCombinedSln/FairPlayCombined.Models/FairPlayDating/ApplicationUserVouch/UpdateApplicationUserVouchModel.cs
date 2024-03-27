using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch
{
    public class UpdateApplicationUserVouchModel : IUpdateModel
    {
        [Required]
        [DeniedValues(0)]
        public long? ApplicationUserVouchId { get; set; }

        [Required]
        public string? FromApplicationUserId { get; set; }

        [Required]
        public string? ToApplicationUserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }
    }
}
