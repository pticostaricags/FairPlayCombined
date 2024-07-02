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
            var executionStrategy = dbContext.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);

                var videoInfoEntity = await dbContext.VideoInfo
                    .AsNoTracking()
                    .SingleAsync(p => p.VideoInfoId == videoInfoId, cancellationToken);

                if (videoInfoEntity is not null)
                {
                    // Delete related entities first
                    await dbContext.VideoCaptions
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoComment
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoDigitalMarketingDailyPosts
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoDigitalMarketingPlan
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoFaceThumbnail
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoIndexingTransaction
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoInfographic
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoJob
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoKeyword
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoTopic
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoWatchTime
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    await dbContext.VideoThumbnail
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    // Delete the main video info
                    await dbContext.VideoInfo
                        .Where(p => p.VideoInfoId == videoInfoId)
                        .ExecuteDeleteAsync(cancellationToken);

                    // Commit the transaction
                    await transaction.CommitAsync(cancellationToken);

                    // Call the Azure Video Indexer service to delete the video by ID
                    var armAccessToken = await azureVideoIndexerService.AuthenticateToAzureArmAsync();
                    var getAccessTokenResult = await azureVideoIndexerService.GetAccessTokenForArmAccountAsync(armAccessToken, cancellationToken);
                    await azureVideoIndexerService.DeleteVideoByIdAsync(videoInfoEntity.VideoId, getAccessTokenResult!.AccessToken!, cancellationToken);
                }
            });
        }

    }
}
