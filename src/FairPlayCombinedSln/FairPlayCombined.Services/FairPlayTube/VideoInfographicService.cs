using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfographic;
using FairPlayCombined.Models.FairPlayTube.VideoThumbnail;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateVideoInfographicModel,
        UpdateVideoInfographicModel,
        VideoInfographicModel,
        FairPlayCombinedDbContext,
        VideoInfographic,
        PaginationRequest,
        PaginationOfT<VideoInfographicModel>
        >]
    public partial class VideoInfographicService : BaseService, IVideoInfographicService
    {
        public async Task<PaginationOfT<VideoInfographicModel>> GetPaginatedVideoInfographicByVideoInfoIdAsync(long videoInfoId, PaginationRequest paginationRequest, CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedVideoInfographicByVideoInfoIdAsync));
            PaginationOfT<VideoInfographicModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoInfographic
                .AsNoTracking()
                .AsSplitQuery()
                .Where(p => p.VideoInfoId == videoInfoId)
                .Select(p => new VideoInfographicModel
                {
                    VideoInfographicId = p.VideoInfographicId,
                    VideoInfoId = p.VideoInfoId,
                    PhotoId = p.PhotoId
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
