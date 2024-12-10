using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBlogsSchema;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayBlogs;


[ServiceOfT<
        CreateBlogPostModel,
        UpdateBlogPostModel,
        BlogPostModel,
        FairPlayCombinedDbContext,
        BlogPost,
        PaginationRequest,
        PaginationOfT<BlogPostModel>
        >]
public partial class BlogPostService: BaseService, IBlogPostService
{
}
