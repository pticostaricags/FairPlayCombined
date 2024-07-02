using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.Common;
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
        private IAzureVideoIndexerService azureVideoIndexerService;
        public VideoInfoService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
            ILogger<VideoInfoService> logger, IAzureVideoIndexerService azureVideoIndexerService) :
            this(dbContextFactory, logger)
        {
            this.azureVideoIndexerService = azureVideoIndexerService;
        }
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
                    RowCreationDateTime = p.RowCreationDateTime,
                    IndexingCost = p.VideoIndexingTransaction.Sum(it => it.IndexingCost)
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
                .AsNoTracking()
                .AsSplitQuery()
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
                PublishedUrl = p.PublishedUrl,
                VideoTopics = p.VideoTopic.Select(p => p.Topic).ToArray(),
                VideoKeywords = p.VideoKeyword.Select(p => p.Keyword).ToArray()
            })
            .SingleOrDefaultAsync(cancellationToken);
            return result;
        }

        public async Task DeleteMyVideoAsync(long videoInfoId, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var videoInfoQuery = dbContext.VideoInfo
                .AsNoTracking()
                .AsSplitQuery()
                .Include(p => p.VideoCaptions)
                .Include(p => p.VideoComment)
                .Include(p => p.VideoDigitalMarketingDailyPosts)
                .Include(p => p.VideoDigitalMarketingPlan)
                .Include(p => p.VideoFaceThumbnail).ThenInclude(p => p.Photo)
                .Include(p => p.VideoIndexingTransaction)
                .Include(p => p.VideoInfographic).ThenInclude(p => p.Photo)
                .Include(p => p.VideoJob)
                .Include(p => p.VideoKeyword)
                .Include(p => p.VideoTopic)
                .Include(p => p.VideoThumbnail)
                .ThenInclude(p => p.Photo)
                .Include(p => p.VideoWatchTime)
                .Where(p => p.VideoInfoId == videoInfoId);
            if (await videoInfoQuery.AnyAsync(cancellationToken))
            {
                foreach (var singleVideoInfo in videoInfoQuery)
                {
                    RemoveVideoCaptions(dbContext, singleVideoInfo);
                    RemoveVideoComments(dbContext, singleVideoInfo);
                    RemoveVideoDigitalMarketingDailyPosts(dbContext, singleVideoInfo);
                    RemoveVideoDigitalMarketingPlans(dbContext, singleVideoInfo);
                    RemoveVideoFaceThumbnails(dbContext, singleVideoInfo);
                    RemoveVideoIndexingTransactions(dbContext, singleVideoInfo);
                    RemoveVideoInfographics(dbContext, singleVideoInfo);
                    RemoveVideoJobs(dbContext, singleVideoInfo);
                    RemoveVideoKeywords(dbContext, singleVideoInfo);
                    RemoveVideoTopics(dbContext, singleVideoInfo);
                    RemoveVideoWatchTime(dbContext, singleVideoInfo);
                    RemoveVideoThumbnails(dbContext, singleVideoInfo);
                    dbContext.VideoInfo.Remove(singleVideoInfo);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    var armAccessToken = await azureVideoIndexerService.AuthenticateToAzureArmAsync();
                    var getAccessTokenResult = await azureVideoIndexerService.GetAccessTokenForArmAccountAsync(armAccessToken, cancellationToken);
                    await azureVideoIndexerService.DeleteVideoByIdAsync(singleVideoInfo.VideoId,
                        getAccessTokenResult!.AccessToken!, cancellationToken);
                }
            }
        }

        private static void RemoveVideoThumbnails(FairPlayCombinedDbContext dbContext, VideoInfo? singleVideoInfo)
        {
            foreach (var singleVideoThumbnail in singleVideoInfo!.VideoThumbnail)
            {
                dbContext.VideoThumbnail.Remove(singleVideoThumbnail);
                dbContext.Photo.Remove(singleVideoThumbnail.Photo);
            }
        }

        private static void RemoveVideoWatchTime(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoWatchTime in singleVideoInfo.VideoWatchTime)
            {
                dbContext.VideoWatchTime.Remove(singleVideoWatchTime);
            }
        }

        private static void RemoveVideoTopics(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoTopic in singleVideoInfo.VideoTopic)
            {
                dbContext.VideoTopic.Remove(singleVideoTopic);
            }
        }

        private static void RemoveVideoKeywords(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoKeyword in singleVideoInfo.VideoKeyword)
            {
                dbContext.VideoKeyword.Remove(singleVideoKeyword);
            }
        }

        private static void RemoveVideoJobs(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoJob in singleVideoInfo.VideoJob)
            {
                dbContext.VideoJob.Remove(singleVideoJob);
            }
        }

        private static void RemoveVideoInfographics(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoInfographic in singleVideoInfo.VideoInfographic)
            {
                dbContext.VideoInfographic.Remove(singleVideoInfographic);
                dbContext.Photo.Remove(singleVideoInfographic.Photo);
            }
        }

        private static void RemoveVideoIndexingTransactions(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoIndexingTransaction in singleVideoInfo.VideoIndexingTransaction)
            {
                dbContext.VideoIndexingTransaction.Remove(singleVideoIndexingTransaction);
            }
        }

        private static void RemoveVideoFaceThumbnails(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoFaceThumbnail in singleVideoInfo.VideoFaceThumbnail)
            {
                dbContext.VideoFaceThumbnail.Remove(singleVideoFaceThumbnail);
                dbContext.Photo.Remove(singleVideoFaceThumbnail.Photo);
            }
        }

        private static void RemoveVideoDigitalMarketingPlans(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoDigitalMarketingPlan in singleVideoInfo.VideoDigitalMarketingPlan)
            {
                dbContext.VideoDigitalMarketingPlan.Remove(singleVideoDigitalMarketingPlan);
            }
        }

        private static void RemoveVideoDigitalMarketingDailyPosts(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoDigitalMarketingDailyPost in singleVideoInfo.VideoDigitalMarketingDailyPosts)
            {
                dbContext.VideoDigitalMarketingDailyPosts.Remove(singleVideoDigitalMarketingDailyPost);
            }
        }

        private static void RemoveVideoComments(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoComment in singleVideoInfo.VideoComment)
            {
                dbContext.VideoComment.Remove(singleVideoComment);
            }
        }

        private static void RemoveVideoCaptions(FairPlayCombinedDbContext dbContext, VideoInfo singleVideoInfo)
        {
            foreach (var singleVideoCaption in singleVideoInfo.VideoCaptions)
            {
                dbContext.VideoCaptions.Remove(singleVideoCaption);
            }
        }
    }
}
