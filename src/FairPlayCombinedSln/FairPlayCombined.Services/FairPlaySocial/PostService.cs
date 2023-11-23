using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.Models.FairPlaySocial.Post;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace FairPlayCombined.Services.FairPlaySocial
{
    [ServiceOfT<
        CreatePostModel,
        UpdatePostModel,
        PostModel,
        FairPlayCombinedDbContext,
        Post,
        PaginationRequest,
        PaginationOfT<PostModel>
        >]
    public partial class PostService : BaseService
    {
        public async Task<PaginationOfT<PostModel>> GetPaginatedPostWithCustomProjectionAsync(
    PaginationRequest paginationRequest,
    Expression<Func<Post, PostModel>> customProjection,
    CancellationToken cancellationToken
    )
        {
            PaginationOfT<PostModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.Post
                .Select(customProjection);
            if (!String.IsNullOrEmpty(orderByString))
                query = query.OrderBy(orderByString);
            result.TotalItems = await query.CountAsync(cancellationToken);
            result.PageSize = paginationRequest.PageSize;
            result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
            result.Items = await query
                .OrderByDescending(p=>p.PostId)
            .Skip(paginationRequest.StartIndex)
            .Take(paginationRequest.PageSize)
            .ToArrayAsync(cancellationToken);
            return result;
        }
    }
}
