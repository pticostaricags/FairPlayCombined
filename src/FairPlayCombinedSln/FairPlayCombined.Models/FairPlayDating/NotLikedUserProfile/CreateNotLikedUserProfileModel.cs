using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.NotLikedUserProfile
{
    public class CreateNotLikedUserProfileModel : ICreateModel
    {
        [Required]
        public string? NotLikingApplicationUserId { get; set; }

        [Required]
        public string? NotLikedApplicationUserId { get; set; }
    }
}
