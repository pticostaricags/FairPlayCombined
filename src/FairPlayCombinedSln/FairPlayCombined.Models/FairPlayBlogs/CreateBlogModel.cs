using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayBlogs;

public class CreateBlogModel : ICreateModel
{
    [CustomRequired]
    [CustomStringLength(50)]
    public string? Name { get; set; }

    [CustomRequired]
    [CustomStringLength(500)]
    public string? Description { get; set; }
    [CustomRequired]
    public long HeaderPhotoId { get; set; }

    [CustomStringLength(100)]
    public string? CustomDomain { get; set; }

    public bool IsCustomDomainVerified { get; set; }

    [CustomRequired]
    [CustomStringLength(450)]
    public string? OwnerApplicationUserId { get; set; }
}
