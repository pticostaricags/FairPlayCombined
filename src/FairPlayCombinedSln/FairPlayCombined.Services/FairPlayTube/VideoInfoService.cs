using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace FairPlayCombined.Services.FairPlayTube
{
    [ServiceOfT<
        CreateVideoInfoModel,
        UpdateVideoInfoModel,
        VideoInfoModel,
        FairPlayCombinedDbContext,
        VideoInfo,
        PaginationRequest,
        PaginationOfT<VideoInfoModel>
        >]
    public partial class VideoInfoService : BaseService
    {
        public async Task<PaginationOfT<VideoInfoModel>> GetPaginatedCompletedVideoInfoAsync(
    PaginationRequest paginationRequest,
    CancellationToken cancellationToken
    )
        {
            PaginationOfT<VideoInfoModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoInfo
                .Where(p=>p.VideoIndexStatusId == (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed)
                .Select(p => new VideoInfoModel
                {
                    VideoInfoId = p.VideoInfoId,
                    AccountId = p.AccountId,
                    VideoId = p.VideoId,
                    Location = p.Location,
                    Name = p.Name,
                    Description = p.Description,
                    FileName = p.FileName,
                    VideoBloblUrl = p.VideoBloblUrl,
                    IndexedVideoUrl = p.IndexedVideoUrl,
                    ApplicationUserId = p.ApplicationUserId,
                    VideoIndexStatusId = p.VideoIndexStatusId,
                    VideoDurationInSeconds = p.VideoDurationInSeconds,
                    VideoIndexSourceClass = p.VideoIndexSourceClass,
                    Price = p.Price,
                    ExternalVideoSourceUrl = p.ExternalVideoSourceUrl,
                    VideoLanguageCode = p.VideoLanguageCode,
                    VideoVisibilityId = p.VideoVisibilityId,
                    ThumbnailUrl = p.ThumbnailUrl,

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
