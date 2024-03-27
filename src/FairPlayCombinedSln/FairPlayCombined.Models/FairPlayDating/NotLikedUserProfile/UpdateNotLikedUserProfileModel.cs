using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.NotLikedUserProfile
{
    public class UpdateNotLikedUserProfileModel : IUpdateModel
    {
        [Required]
        public long? NotLikedUserProfileId { get; set; }

        [Required]
        public string? NotLikingApplicationUserId { get; set; }

        [Required]
        public string? NotLikedApplicationUserId { get; set; }
    }
}
