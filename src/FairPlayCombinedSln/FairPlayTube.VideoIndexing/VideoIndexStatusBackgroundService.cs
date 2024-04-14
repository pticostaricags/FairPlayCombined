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
        try
        {
            TimeSpan timeToWait = TimeSpan.FromMinutes(1);
            var scope = serviceScopeFactory.CreateScope();
            var dbContextFactory = scope.ServiceProvider
                .GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
            var azureVideoIndexerService =
                scope.ServiceProvider.GetRequiredService<AzureVideoIndexerService>();
            GetAccessTokenResponseModel? getviTokenResult = null;
            if (!stoppingToken.IsCancellationRequested)
            {
                getviTokenResult = await AuthenticateAsync(azureVideoIndexerService, stoppingToken);
                var viSupportedLanguages = await azureVideoIndexerService
                    .GetSupportedLanguagesAsync(getviTokenResult!.AccessToken!, stoppingToken);
                if (viSupportedLanguages != null && viSupportedLanguages?.Length > 0)
                {
                    foreach (var singleViSupportedLanguage in viSupportedLanguages!)
                    {
                        if (await dbContext.VideoIndexerSupportedLanguage
                            .SingleOrDefaultAsync(p => p.LanguageCode == singleViSupportedLanguage.languageCode,
                            stoppingToken) is null)
                        {
                            await dbContext.VideoIndexerSupportedLanguage.AddAsync(
                                new VideoIndexerSupportedLanguage()
                                {
                                    LanguageCode = singleViSupportedLanguage.languageCode,
                                    Name = singleViSupportedLanguage.name
                                }, stoppingToken);
                            await dbContext.SaveChangesAsync(stoppingToken);
                        }
                    }
                }
            }
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
                    var videosIndex = await azureVideoIndexerService.SearchVideosByIdsAsync(
                        getviTokenResult!.AccessToken!, allVideosInProcessingStatus, stoppingToken);
                    LogVideoIndexStatus(logger, videosIndex);
                    var processingVideos = videosIndex?.results?.Where(p => p.state ==
                    FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processing.ToString());
                    if (processingVideos?.Count() > 0)
                    {
                        foreach (var singleProcessingVideo in processingVideos)
                        {
                            var singleVideoIndex = await azureVideoIndexerService.GetVideoIndexAsync(
                                singleProcessingVideo.id!, getviTokenResult.AccessToken!,
                                cancellationToken: stoppingToken);
                            var videoEntity = await dbContext.VideoInfo.SingleAsync(p => p.VideoId == singleProcessingVideo.id,
                                cancellationToken: stoppingToken);
                            if (!String.IsNullOrWhiteSpace(singleVideoIndex!.videos![0].processingProgress))
                            {
                                int processingProgress = Convert.ToInt32(singleVideoIndex!.videos![0].processingProgress!.Trim('%'));
                                logger.LogInformation("Updating processingProgress for " +
                                    "video:{videoId}. processingProgress:{processingProgress}",
                                    singleVideoIndex.id, singleVideoIndex!.videos![0].processingProgress);
                                //avoid doing a database call if processingProgress value has not changed
                                if (processingProgress != videoEntity.VideoIndexingProcessingPercentage)
                                {
                                    videoEntity.VideoIndexingProcessingPercentage = processingProgress;
                                    await dbContext.SaveChangesAsync(cancellationToken: stoppingToken);
                                }
                            }
                        }
                    }
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
                            var thumbnailBytes = await azureVideoIndexerService
                                .GetVideoThumbnailAsync(singleVideoEntity.VideoId,
                                indexCompleteVideos.Single(p => p.id == singleVideoEntity.VideoId)
                                .thumbnailId!,
                                getviTokenResult.AccessToken!, stoppingToken);
                            singleVideoEntity.VideoThumbnailPhoto = new()
                            {
                                Filename = $"Thumbnail-{singleVideoEntity.VideoId}.jpg",
                                Name = $"Thumbnail-{singleVideoEntity.VideoId}",
                                PhotoBytes = thumbnailBytes
                            };
                            await dbContext.VideoIndexingTransaction.AddAsync(new VideoIndexingTransaction()
                            {
                                VideoInfoId = singleVideoEntity.VideoInfoId,
                                IndexingCost = costPerMinute * ((decimal)singleVideoEntity.VideoDurationInSeconds / 60)
                            }, stoppingToken);
                            singleVideoEntity.VideoIndexStatusId = (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed;
                            singleVideoEntity.VideoIndexingProcessingPercentage = 100;
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
        catch (Exception ex)
        {
            logger.LogCritical(exception: ex, "Error: {errorMessage}", ex.Message);
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