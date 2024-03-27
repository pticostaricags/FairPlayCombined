using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Models.AzureVideoIndexer;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FairPlayTube.VideoIndexing;

public class VideoIndexStatusBackgroundService(ILogger<VideoIndexStatusBackgroundService> logger,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan timeToWait = TimeSpan.FromMinutes(5);
        var scope = serviceScopeFactory.CreateScope();
        var dbContextFactory = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
        var azureVideoIndexerService =
            scope.ServiceProvider.GetRequiredService<AzureVideoIndexerService>();
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            string[] allVideosInProcessingStatus =
                await GetAllVideosInProcessingStatusAsync(dbContext, stoppingToken);
            if (allVideosInProcessingStatus.Length != 0)
            {
                GetAccessTokenResponseModel? getviTokenResult = await AuthenticateAsync(azureVideoIndexerService, stoppingToken);
                var videosIndex = await azureVideoIndexerService.SearchVideosByIdsAsync(
                    getviTokenResult!.AccessToken!, allVideosInProcessingStatus, stoppingToken);
                LogVideoIndexStatus(logger, videosIndex);
                var indexCompleteVideos = videosIndex?.results?.Where(p => p.state ==
                FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed.ToString());
                if (indexCompleteVideos?.Count() > 0)
                {
                    var indexCompleteVideosIds = indexCompleteVideos.Select(p => p.id).ToArray();
                    var query = dbContext.VideoInfo
                        .Include(p => p.ApplicationUser).Where(p => indexCompleteVideosIds.Contains(p.VideoId));

                    var costPerMinute = dbContext.VideoIndexingCost
                        .OrderByDescending(d => d.RowCreationDateTime)
                        .First()
                        .CostPerMinute;

                    foreach (var singleVideoEntity in query)
                    {
                        await dbContext.VideoIndexingTransaction.AddAsync(new VideoIndexingTransaction()
                        {
                            VideoInfoId = singleVideoEntity.VideoInfoId,
                            IndexingCost = costPerMinute * ((decimal)singleVideoEntity.VideoDurationInSeconds / 60)
                        }, stoppingToken);
                        singleVideoEntity.VideoIndexStatusId = (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed;
                        singleVideoEntity.VideoDurationInSeconds =
                            videosIndex!.results!
                            .Single(p => p.id == singleVideoEntity.VideoId).durationInSeconds;
                        var completedVideoIndex = await azureVideoIndexerService.GetVideoIndexAsync(
                            singleVideoEntity.VideoId, getviTokenResult.AccessToken!,
                            stoppingToken);
                        singleVideoEntity.VideoIndexJson = JsonSerializer.Serialize(completedVideoIndex);
                        InsertInsights(singleVideoEntity, completedVideoIndex);
                    }

                    await dbContext.SaveChangesAsync(cancellationToken: stoppingToken);
                }
            }
            logger.LogInformation("Current Iteration finished at: {time}. Next Iteration at {time2}", DateTimeOffset.Now, DateTimeOffset.Now.Add(timeToWait));
            await Task.Delay(timeToWait, stoppingToken);
        }
    }

    private static void InsertInsights(VideoInfo? singleVideoEntity, GetVideoIndexResponseModel? completedVideoIndex)
    {
        if (completedVideoIndex?.summarizedInsights?.topics?.Length > 0)
            foreach (var singleTopic in completedVideoIndex.summarizedInsights.topics)
            {
                VideoTopic videoTopicEntity = new()
                {
                    Confidence = singleTopic.confidence,
                    Topic = singleTopic.name
                };
                singleVideoEntity!.VideoTopic.Add(videoTopicEntity);
            }
        if (completedVideoIndex?.summarizedInsights?.keywords?.Length > 0)
        {
            foreach (var singleKeyword in completedVideoIndex.summarizedInsights.keywords
                .DistinctBy(p => p.name))
            {
                VideoKeyword videoKeywordEntity = new()
                {
                    Keyword = singleKeyword.name
                };
                singleVideoEntity!.VideoKeyword.Add(videoKeywordEntity);
            }
        }
    }

    private static void LogVideoIndexStatus(ILogger<VideoIndexStatusBackgroundService> logger, SearchVideosResponseModel? videosIndex)
    {
        if (videosIndex?.results?.Length > 0)
        {
            foreach (var singleVideoIndex in videosIndex.results)
            {
                logger.LogInformation("VideoId: {videoId}. " +
                    "Status in VI: {statusInVI}. " +
                    "Progress: {progress}", singleVideoIndex.id,
                    singleVideoIndex.state,
                    singleVideoIndex.processingProgress);
            }
        }
    }

    private static async Task<GetAccessTokenResponseModel?> AuthenticateAsync(AzureVideoIndexerService azureVideoIndexerService, CancellationToken stoppingToken)
    {
        var armAccessToken = await azureVideoIndexerService.AuthenticateToAzureArmAsync();
        var getviTokenResult = await azureVideoIndexerService.GetAccessTokenForArmAccountAsync(armAccessToken, stoppingToken);
        return getviTokenResult;
    }

    private static async Task<string[]> GetAllVideosInProcessingStatusAsync(FairPlayCombinedDbContext dbContext, CancellationToken stoppingToken)
    {
        return await dbContext.VideoInfo.
                        Where(p => p.VideoIndexStatusId ==
                        (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processing)
                        .Select(p => p.VideoId)
                        .ToArrayAsync(stoppingToken);
    }
}