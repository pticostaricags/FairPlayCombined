using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayBlogs.BlogPost;

public class CreateBlogPostModel : ICreateModel
{
    [DeniedValues(default(long))]
    public long BlogId { get; set; }

    [CustomRequired]
    [CustomStringLength(50)]
    public string? Title { get; set; }

    [CustomRequired]
    [CustomStringLength(250)]
    public string? PreviewText { get; set; }

    [CustomRequired]
    public string? Content { get; set; }

    public long ThumbnailPhotoId { get; set; }

    public int BlogPostStatusId { get; set; }
}
