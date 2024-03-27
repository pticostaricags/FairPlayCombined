using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch
{
    public class CreateApplicationUserVouchModel : ICreateModel
    {

        [Required]
        public string? FromApplicationUserId { get; set; }

        [Required]
        public string? ToApplicationUserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }
    }
}