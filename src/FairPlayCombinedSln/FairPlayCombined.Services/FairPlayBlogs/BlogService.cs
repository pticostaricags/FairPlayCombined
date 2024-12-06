using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBlogsSchema;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayBlogs
{
    [ServiceOfT<
        CreateBlogModel,
        UpdateBlogModel,
        BlogModel,
        FairPlayCombinedDbContext,
        Blog,
        PaginationRequest,
        PaginationOfT<BlogModel>
        >]
    public partial class BlogService : BaseService, IBlogService
    {
    }
}
