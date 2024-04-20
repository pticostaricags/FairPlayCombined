using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Models.FairPlayTube.VideoThumbnail;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
    CreateVideoThumbnailModel,
    UpdateVideoThumbnailModel,
    VideoThumbnailModel,
    FairPlayCombinedDbContext,
    VideoThumbnail,
    PaginationRequest,
    PaginationOfT<VideoThumbnailModel>
    >]
    public partial class VideoThumbnailService : BaseService
    {
        public async Task<PaginationOfT<VideoThumbnailModel>>
            GetPaginatedVideoThumbnailByVideoInfoIdAsync(long videoInfoId,
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedVideoThumbnailAsync));
            PaginationOfT<VideoThumbnailModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoThumbnail
                .Where(p=>p.VideoInfoId == videoInfoId)
                .Select(p => new FairPlayCombined.Models.FairPlayTube.VideoThumbnail.VideoThumbnailModel
                {
                    VideoThumbnailId = p.VideoThumbnailId,
                    VideoInfoId = p.VideoInfoId,
                    PhotoId = p.PhotoId,
                    PhotoBytes = p.Photo.PhotoBytes
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
