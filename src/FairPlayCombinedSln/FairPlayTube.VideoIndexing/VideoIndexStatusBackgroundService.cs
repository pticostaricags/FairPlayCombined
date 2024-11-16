using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.AzureVideoIndexer;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text.Json;

namespace FairPlayTube.VideoIndexing;

public class VideoIndexStatusBackgroundService(ILogger<VideoIndexStatusBackgroundService> logger,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly HttpClient httpClient = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TimeSpan timeToWait = TimeSpan.FromMinutes(5);
        var scope = serviceScopeFactory.CreateScope();
        var dbContextFactory = scope.ServiceProvider
            .GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
        var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
        var azureVideoIndexerService =
            scope.ServiceProvider.GetRequiredService<IAzureVideoIndexerService>();
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                GetAccessTokenResponseModel? getviTokenResult = null;
                if (!stoppingToken.IsCancellationRequested)
                {
                    getviTokenResult = await AuthenticateAsync(azureVideoIndexerService, stoppingToken);
                    await PrepareSupportedLanguagesAsync(dbContext, azureVideoIndexerService, getviTokenResult, stoppingToken);
                }
                if (logger.IsEnabled(LogLevel.Information))
                {
                    logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
                }
                string[] allVideosInProcessingStatus =
                    await GetAllVideosInProcessingStatusAsync(dbContext, stoppingToken);
                if (allVideosInProcessingStatus.Length != 0)
                {
                    try
                    {
                        var videosIndex = await azureVideoIndexerService.SearchVideosByIdsAsync(
                            getviTokenResult!.AccessToken!, allVideosInProcessingStatus, stoppingToken);
                        LogVideoIndexStatus(logger, videosIndex);
                        var processingVideos = videosIndex?.results?.Where(p => p.state ==
                        FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processing.ToString());
                        if (processingVideos?.Any() == true)
                        {
                            await UpdateProcessingPercentageAsync(logger, dbContext, azureVideoIndexerService, getviTokenResult, processingVideos, stoppingToken);
                        }
                        var indexCompleteVideos = videosIndex?.results?.Where(p => p.state ==
                        FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed.ToString());
                        await UpdateVideoIndexingTransactionAsync(dbContext,
                            azureVideoIndexerService, getviTokenResult, indexCompleteVideos,
                            stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        await dbContext.ErrorLog.AddAsync(new()
                        {
                            FullException = ex.ToString(),
                            Message = ex.Message,
                            StackTrace = ex.StackTrace
                        }, stoppingToken);
                        await dbContext.SaveChangesAsync(stoppingToken);
                        logger.LogError(exception: ex, "Error: {ErrorMessage}", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogCritical(exception: ex, "Error: {ErrorMessage}", ex.Message);
            }
            logger.LogInformation("Current Iteration finished at: {Time}. Next Iteration at {Time2}", DateTimeOffset.Now, DateTimeOffset.Now.Add(timeToWait));
            await Task.Delay(timeToWait, stoppingToken);
        }
    }

    private async Task UpdateVideoIndexingTransactionAsync(
        FairPlayCombinedDbContext dbContext,
        IAzureVideoIndexerService azureVideoIndexerService,
        GetAccessTokenResponseModel? getviTokenResult,
        IEnumerable<Result>? indexCompleteVideos,
        CancellationToken stoppingToken)
    {
        if (indexCompleteVideos?.Any() == true)
        {
            var indexCompleteVideosIds = indexCompleteVideos.Select(p => p.id).ToArray();
            var query = dbContext.VideoInfo
                .Include(p => p.ApplicationUser).Where(p => indexCompleteVideosIds.Contains(p.VideoId));

            var costPerMinute = (await dbContext.VideoIndexingCost
                .OrderByDescending(d => d.RowCreationDateTime)
                .FirstAsync(stoppingToken))
                .CostPerMinute;

            foreach (var singleVideoEntity in query)
            {
                var thumbnailBytes = await azureVideoIndexerService
                    .GetVideoThumbnailAsync(singleVideoEntity.VideoId,
                    indexCompleteVideos.Single(p => p.id == singleVideoEntity.VideoId)
                    .thumbnailId!,
                    getviTokenResult!.AccessToken!, stoppingToken);
                singleVideoEntity.VideoThumbnailPhoto = new()
                {
                    Filename = $"Thumbnail-{singleVideoEntity.VideoId}.jpg",
                    Name = $"Thumbnail-{singleVideoEntity.VideoId}",
                    PhotoBytes = thumbnailBytes
                };
                var completedVideoIndex = await azureVideoIndexerService.GetVideoIndexAsync(
                    singleVideoEntity.VideoId, getviTokenResult.AccessToken!,
                    stoppingToken);
                var userFundsEntity = await dbContext.UserFunds
                    .SingleAsync(p => p.ApplicationUserId ==
                    singleVideoEntity.ApplicationUserId, stoppingToken);
                var videoIndexingMarginEntity =
                    await dbContext.VideoIndexingMargin.SingleAsync(stoppingToken);
                var indexingCost = costPerMinute * ((decimal)completedVideoIndex!.durationInSeconds / 60);
                var indexingCostWithMargin = indexingCost + (indexingCost * videoIndexingMarginEntity.Margin);
                userFundsEntity.AvailableFunds -= indexingCostWithMargin;
                await dbContext.VideoIndexingTransaction.AddAsync(new VideoIndexingTransaction()
                {
                    VideoInfoId = singleVideoEntity.VideoInfoId,
                    IndexingCost = indexingCostWithMargin
                }, stoppingToken);
                singleVideoEntity.VideoIndexStatusId = (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed;
                singleVideoEntity.VideoIndexingProcessingPercentage = 100;
                singleVideoEntity.VideoDurationInSeconds = completedVideoIndex.durationInSeconds;
                singleVideoEntity.PublishedUrl = completedVideoIndex!.videos![0].publishedUrl;
                singleVideoEntity.VideoIndexJson = JsonSerializer.Serialize(completedVideoIndex);
                await InsertVideoFaceThumbnailsAsync(azureVideoIndexerService, getviTokenResult, singleVideoEntity, completedVideoIndex, stoppingToken);
                InsertInsights(singleVideoEntity, completedVideoIndex);
            }

            await dbContext.SaveChangesAsync(cancellationToken: stoppingToken);
        }
    }

    private async Task InsertVideoFaceThumbnailsAsync(IAzureVideoIndexerService azureVideoIndexerService, GetAccessTokenResponseModel getviTokenResult, VideoInfo? singleVideoEntity, GetVideoIndexResponseModel? completedVideoIndex, CancellationToken stoppingToken)
    {
        var facesThumbnailsDownloadUrl =
                            await azureVideoIndexerService
                            .GetFacesThumbnailsDownloadUrlAsync(completedVideoIndex!.id!,
                            getviTokenResult.AccessToken!, stoppingToken);
        List<(string PersonName, string ThumbnailFilename)> pairs =
                completedVideoIndex!.GetPersonThumbnailPairs();
        if (!String.IsNullOrWhiteSpace(facesThumbnailsDownloadUrl))
        {
            facesThumbnailsDownloadUrl =
                facesThumbnailsDownloadUrl.Trim('"');
        }
        var stream = await httpClient!
            .GetStreamAsync(facesThumbnailsDownloadUrl, stoppingToken);
        ZipArchive archive = new(stream);
        foreach (ZipArchiveEntry entry in archive.Entries)
        {
            var entryStream = entry.Open();
            MemoryStream entryMemoryStream = new();
            await entryStream.CopyToAsync(entryMemoryStream, stoppingToken);
            byte[] fileBytes = entryMemoryStream.ToArray();
            string faceName = string.Empty;
            foreach (var (_, ThumbnailFilename) in pairs)
            {
                if (entry.FullName == ThumbnailFilename)
                {
                    faceName = ThumbnailFilename;
                }
            }
            if (!String.IsNullOrWhiteSpace(faceName) &&
                !singleVideoEntity!.VideoFaceThumbnail
                .Any(p => p.FaceName == faceName))
            {
                singleVideoEntity.VideoFaceThumbnail.Add(new()
                {
                    FaceName = faceName,
                    Photo = new()
                    {
                        Filename = entry.Name,
                        Name = Path.GetFileNameWithoutExtension(entry.Name),
                        PhotoBytes = fileBytes
                    }
                });
            }
        }
    }

    private static async Task UpdateProcessingPercentageAsync(ILogger<VideoIndexStatusBackgroundService> logger,
        FairPlayCombinedDbContext dbContext, IAzureVideoIndexerService azureVideoIndexerService, GetAccessTokenResponseModel? getviTokenResult, IEnumerable<Result>? processingVideos, CancellationToken stoppingToken)
    {
        foreach (var singleProcessingVideoId in processingVideos!.Select(p => p.id!))
        {
            var singleVideoIndex = await azureVideoIndexerService.GetVideoIndexAsync(
                singleProcessingVideoId, getviTokenResult!.AccessToken!,
                cancellationToken: stoppingToken);

            var videoEntity = await dbContext.VideoInfo.SingleAsync(p => p.VideoId == singleProcessingVideoId,
                cancellationToken: stoppingToken);
            if (!String.IsNullOrWhiteSpace(singleVideoIndex!.videos![0].processingProgress))
            {
                int processingProgress = Convert.ToInt32(singleVideoIndex!.videos![0].processingProgress!.Trim('%'));
                logger.LogInformation("Updating processingProgress for " +
                    "video:{VideoId}. processingProgress:{ProcessingProgress}",
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

    private static async Task PrepareSupportedLanguagesAsync(FairPlayCombinedDbContext dbContext,
        IAzureVideoIndexerService azureVideoIndexerService, GetAccessTokenResponseModel? getviTokenResult, CancellationToken stoppingToken)
    {
        var viSupportedLanguages = await azureVideoIndexerService
                            .GetSupportedLanguagesAsync(getviTokenResult!.AccessToken!, stoppingToken);
        if (viSupportedLanguages?.Length > 0)
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

    private static void InsertInsights(VideoInfo? singleVideoEntity,
        GetVideoIndexResponseModel? completedVideoIndex)
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
                logger.LogInformation("VideoId: {VideoId}. " +
                    "Status in VI: {StatusInVI}. " +
                    "Progress: {Progress}", singleVideoIndex.id,
                    singleVideoIndex.state,
                    singleVideoIndex.processingProgress);
            }
        }
    }

    private static async Task<GetAccessTokenResponseModel?> AuthenticateAsync(
        IAzureVideoIndexerService azureVideoIndexerService, CancellationToken stoppingToken)
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