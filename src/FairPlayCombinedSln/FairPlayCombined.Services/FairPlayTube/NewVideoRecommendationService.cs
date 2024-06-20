using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateNewVideoRecommendationModel,
        UpdateNewVideoRecommendationModel,
        NewVideoRecommendationModel,
        FairPlayCombinedDbContext,
        NewVideoRecommendation,
        PaginationRequest,
        PaginationOfT<NewVideoRecommendationModel>
        >]
    public partial class NewVideoRecommendationService : BaseService, INewVideoRecommendationService
    {
        public async Task<PaginationOfT<NewVideoRecommendationModel>> GetPaginatedNewVideoRecommendationForUserIdAsync(PaginationRequest paginationRequest, string userId, CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedNewVideoRecommendationAsync));
            PaginationOfT<NewVideoRecommendationModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.NewVideoRecommendation
                .Where(p=>p.ApplicationUserId == userId)
                .AsNoTracking()
                .AsSplitQuery()
                .Select(p => new FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation.NewVideoRecommendationModel
                {
                    NewVideoRecommendationId = p.NewVideoRecommendationId,
                    ApplicationUserId = p.ApplicationUserId,
                    HtmlNewVideoRecommendation = p.HtmlNewVideoRecommendation,

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
