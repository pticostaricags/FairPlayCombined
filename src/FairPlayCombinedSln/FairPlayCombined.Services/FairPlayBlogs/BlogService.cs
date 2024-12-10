using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBlogsSchema;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs.Blog;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

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
        public async Task<PaginationOfT<BlogModel>> GetAllBlogByUserIdAsync(string userId,
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetAllBlogByUserIdAsync));
            PaginationOfT<BlogModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.Blog
                .Where(p=>p.OwnerApplicationUserId == userId)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(p => new BlogModel
                {
                    BlogId = p.BlogId,
                    CustomDomain=p.CustomDomain,
                    Description = p.Description,
                    HeaderPhotoId=p.HeaderPhotoId,
                    IsCustomDomainVerified=p.IsCustomDomainVerified,
                    Name = p.Name,
                    OwnerApplicationUserId=p.OwnerApplicationUserId
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
}
