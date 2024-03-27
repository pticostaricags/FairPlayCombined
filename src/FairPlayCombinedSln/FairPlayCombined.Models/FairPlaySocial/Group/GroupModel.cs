using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlaySocial.Group
{
    public class GroupModel : IListModel
    {
        public long GroupId { get; set; }
        public string? OwnerApplicationUserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? TopicTag { get; set; }
    }
}
