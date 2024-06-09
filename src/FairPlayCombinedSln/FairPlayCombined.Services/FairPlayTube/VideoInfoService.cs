using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    public partial class VideoInfoService : BaseService, IVideoInfoService
    {
        public async Task<PaginationOfT<VideoInfoModel>> GetPaginatedNotCompletedVideoInfobyUserIdAsync(
            PaginationRequest paginationRequest,
            string userId,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedNotCompletedVideoInfobyUserIdAsync));
            PaginationOfT<VideoInfoModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoInfo
                .Include(p => p.VideoKeyword)
                .Include(p => p.VideoTopic)
                .Include(p => p.VideoCaptions)
                .Where(p => p.VideoIndexStatusId != (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed
                && p.ApplicationUserId == userId)
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
                    YouTubeVideoId = p.YouTubeVideoId,
                    VideoKeywords = p.VideoKeyword.Select(p => p.Keyword).ToArray(),
                    VideoTopics = p.VideoTopic.Select(p => p.Topic).ToArray(),
                    EnglishCaptions = p.VideoCaptions.Where(p => p.Language == "en-US")
                    .Select(p => p.Content).SingleOrDefault(),
                    VideoIndexingProcessingPercentage = p.VideoIndexingProcessingPercentage
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
        public async Task<PaginationOfT<VideoInfoModel>> GetPaginatedCompletedVideoInfobyUserIdAsync(
            PaginationRequest paginationRequest,
            string userId,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetPaginatedCompletedVideoInfobyUserIdAsync));
            PaginationOfT<VideoInfoModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.VideoInfo
                .AsNoTracking()
                .AsSplitQuery()
                .Where(p => p.VideoIndexStatusId == (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed
                && p.ApplicationUserId == userId)
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
                    YouTubeVideoId = p.YouTubeVideoId,
                    VideoKeywords = p.VideoKeyword.Select(p => p.Keyword).ToArray(),
                    VideoTopics = p.VideoTopic.Select(p => p.Topic).ToArray(),
                    EnglishCaptions = p.VideoCaptions.Where(p => p.Language == "en-US")
                    .Select(p => p.Content).SingleOrDefault(),
                    VideoIndexingProcessingPercentage = p.VideoIndexingProcessingPercentage,
                    RowCreationDateTime = p.RowCreationDateTime
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
                .Where(p => p.VideoIndexStatusId == (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed)
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
                    YouTubeVideoId = p.YouTubeVideoId,
                    LifetimeViewers = p.VideoWatchTime.Select(p => p.WatchedByApplicationUserId).Distinct().Count(),
                    LifetimeSessions = p.VideoWatchTime.Count,
                    LifetimeWatchTime = TimeSpan.FromSeconds(p.VideoWatchTime.Sum(p => p.WatchTime)),
                    PublishedOnString = (DateTimeOffset.UtcNow.Subtract(p.RowCreationDateTime).TotalDays < 1 ? "Today" : $"{DateTimeOffset.UtcNow.Subtract(p.RowCreationDateTime).Days} Day(s) ago")
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

        public async Task<VideoInfoModel?> GetVideoInfoByVideoIdAsync(string videoId,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.VideoInfo
            .Where(p => p.VideoId == videoId)
            .AsNoTracking()
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
                YouTubeVideoId = p.YouTubeVideoId,
                PublishedUrl = p.PublishedUrl
            })
            .SingleOrDefaultAsync(cancellationToken);
            return result;
        }
    }
}
