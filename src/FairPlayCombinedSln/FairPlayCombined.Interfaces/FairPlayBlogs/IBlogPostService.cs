using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.FairPlayBlogs;
public interface IBlogPostService
{
    Task<long> CreateBlogPostAsync(CreateBlogPostModel createModel,
        CancellationToken cancellationToken);
    Task<BlogPostModel[]> GetAllBlogPostAsync(CancellationToken cancellationToken);
    Task<BlogPostModel> GetBlogPostByIdAsync(long id, CancellationToken cancellationToken);
    Task<BlogPostModel?> GetBlogPostByBlogNameAndPostTitleAsync(string blogName, string blogPostTitle, CancellationToken cancellationToken);
    Task DeleteBlogPostByIdAsync(long id, CancellationToken cancellationToken);
    Task<PaginationOfT<BlogPostModel>> GetPaginatedBlogPostAsync(
    PaginationRequest paginationRequest, CancellationToken cancellationToken);
    Task<PaginationOfT<BlogPostModel>> GetPaginatedBlogPostByBlogIdAsync(
        long blogId, PaginationRequest paginationRequest, CancellationToken cancellationToken);
}