using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayDating.LikedUserProfile
{
    public class LikedUserProfileModel : IListModel
    {
        public long? LikedUserProfileId { get; set; }
        public string? LikingApplicationUserId { get; set; }
        public string? LikedApplicationUserId { get; set; }
    }
}
