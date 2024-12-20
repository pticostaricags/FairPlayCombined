using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBlogsSchema;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

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
public partial class BlogPostService : BaseService, IBlogPostService
{
    public async Task<BlogPostModel?> GetBlogPostByBlogNameAndPostTitleAsync(
        string blogName, string blogPostTitle, CancellationToken cancellationToken)
    {
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var result = await dbContext.BlogPost
        .Where(p => p.Blog.Name == blogName && p.Title == blogPostTitle)
        .AsNoTracking()
        .Select(p => new BlogPostModel
        {
            BlogPostId = p.BlogPostId,
            BlogId = p.BlogId,
            Title = p.Title,
            PreviewText = p.PreviewText,
            Content = p.Content,
            ThumbnailPhotoId = p.ThumbnailPhotoId,
            BlogPostStatusId = p.BlogPostStatusId,

        })
        .SingleOrDefaultAsync(cancellationToken);
        return result;
    }

    public async Task<PaginationOfT<BlogPostModel>> GetPaginatedBlogPostByBlogIdAsync(
        long blogId, PaginationRequest paginationRequest, CancellationToken cancellationToken)
    {
        logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedBlogPostByBlogIdAsync));
        PaginationOfT<BlogPostModel> result = new();
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        string orderByString = string.Empty;
        if (paginationRequest.SortingItems?.Length > 0)
            orderByString =
                String.Join(",",
                paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
        var query = dbContext.BlogPost
            .Where(p=>p.Blog.BlogId == blogId)
            .Select(p => new BlogPostModel
            {
                BlogPostId = p.BlogPostId,
                BlogId = p.BlogId,
                Title = p.Title,
                PreviewText = p.PreviewText,
                Content = p.Content,
                ThumbnailPhotoId = p.ThumbnailPhotoId,
                BlogPostStatusId = p.BlogPostStatusId,

            });
        if (!String.IsNullOrEmpty(orderByString))
            query = query.OrderBy(orderByString);
        result.TotalItems = await query.CountAsync(cancellationToken);
        result.PageSize = paginationRequest.PageSize;
        result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
        result.Items = await query
        .Skip(paginationRequest.StartIndex)
        .Take(paginationRequest.PageSize)
        .ToArrayAsync(cancellationToken);
        return result;
    }
}
