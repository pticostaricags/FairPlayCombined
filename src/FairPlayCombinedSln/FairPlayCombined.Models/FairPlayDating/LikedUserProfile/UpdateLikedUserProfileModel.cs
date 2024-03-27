using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.LikedUserProfile
{
    public class UpdateLikedUserProfileModel : IUpdateModel
    {
        [Required]
        public long? LikedUserProfileId { get; set; }

        [Required]
        public string? LikingApplicationUserId { get; set; }

        [Required]
        public string? LikedApplicationUserId { get; set; }
    }
}
