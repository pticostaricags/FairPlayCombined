using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayBlogs.BlogPost;

public class BlogPostModel : IListModel
{
    public long BlogPostId { get; set; }

    public long BlogId { get; set; }
    public string? Title { get; set; }
    public string? PreviewText { get; set; }
    public string? Content { get; set; }
    public long ThumbnailPhotoId { get; set; }
    public int BlogPostStatusId { get; set; }
}
