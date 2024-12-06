using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayBlogs
{
    public class BlogModel : IListModel
    {
        public long BlogId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public byte[]? HeaderPhotoBytes { get; set; }
        public string? CustomDomain { get; set; }
        public bool IsCustomDomainVerified { get; set; }
        public string? OwnerApplicationUserId { get; set; }
    }
}
