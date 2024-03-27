using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.LikedUserProfile
{
    public class CreateLikedUserProfileModel : ICreateModel
    {
        [Required]
        public string? LikingApplicationUserId { get; set; }

        [Required]
        public string? LikedApplicationUserId { get; set; }
    }
}
