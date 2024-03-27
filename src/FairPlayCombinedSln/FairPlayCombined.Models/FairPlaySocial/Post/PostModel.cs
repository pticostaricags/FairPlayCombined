using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlaySocial.Post
{
    public class PostModel : IListModel
    {
        public long PostId { get; set; }
        public int PostVisibilityId { get; set; }
        public long? PhotoId { get; set; }
        public int PostTypeId { get; set; }
        public long? ReplyToPostId { get; set; }
        public long? GroupId { get; set; }
        public string? Text { get; set; }
        public string? OwnerApplicationUserId { get; set; }
        public string? OwnerApplicationUserName { get; set; }
    }
}
