using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayDating.NotLikedUserProfile
{
    public class NotLikedUserProfileModel : IListModel
    {
        public long? NotLikedUserProfileId { get; set; }
        public string? NotLikingApplicationUserId { get; set; }
        public string? NotLikedApplicationUserId { get; set; }
    }
}
