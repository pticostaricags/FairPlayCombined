using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace FairPlayTube.VideoIndexing;

public class VideoCaptionsBackgroundService(ILogger<VideoCaptionsBackgroundService> logger,
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
                logger.LogInformation("Worker running at: {Time}", DateTimeOffset.Now);
            }
            var armToken = await azureVideoIndexerService.AuthenticateToAzureArmAsync();
            var getViTokenResponse = await azureVideoIndexerService
                .GetAccessTokenForArmAccountAsync(armToken, stoppingToken);
            var supportedLanguages = await azureVideoIndexerService
                .GetSupportedLanguagesAsync(getViTokenResponse!.AccessToken!, stoppingToken);
            string[] defaultLanguageCodes = ["en-US", "es-MX"];
            foreach (var singleSupportedLanguage in supportedLanguages!.
                Where(p=>defaultLanguageCodes.Contains(p.languageCode)))
            {
                await AddLanguageCaptions(dbContext,
                    azureVideoIndexerService,
                    getViTokenResponse!.AccessToken!,
                    singleSupportedLanguage!.languageCode!,
                    stoppingToken);
            }
            logger.LogInformation("Current Iteration finished at: {Time}. Next Iteration at {Time2}", DateTimeOffset.Now, DateTimeOffset.Now.Add(timeToWait));
            await Task.Delay(timeToWait, stoppingToken);
        }
    }

    private async Task AddLanguageCaptions(
        FairPlayCombinedDbContext dbContext,
        AzureVideoIndexerService azureVideoIndexerService,
        string viAccessToken,
        string language,
        CancellationToken stoppingToken)
    {
        var allProcessedVideosWithNoLanguageCaptions =
                        await dbContext.VideoInfo
                        .Include(p => p.VideoCaptions)
                        .Where(p => !p.VideoCaptions.Any(p => p.Language == language) &&
                        p.VideoIndexStatusId == (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processed)
                        .ToArrayAsync(stoppingToken);
        foreach (var singleVideoWithNoLanguageCaptions in allProcessedVideosWithNoLanguageCaptions)
        {
            logger.LogInformation("Retrieving captions for videoId: {VideoId}. Language: {Language}",
                singleVideoWithNoLanguageCaptions.VideoId,
                language);
            string? videoCaptions = default;
            try
            {
                videoCaptions =
                    await azureVideoIndexerService.GetVideoVTTCaptionsAsync(
                        singleVideoWithNoLanguageCaptions.VideoId,
                        viAccessToken,
                        language, stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(exception: ex, "Exception when retrieving captions: {Exception}", ex.Message);
                if (ex.Message.Contains("Too Many Requests"))
                {
                    logger.LogInformation("Retrying getting captions in 1 minute");
                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
                    try
                    {
                        videoCaptions =
                        await azureVideoIndexerService.GetVideoVTTCaptionsAsync(
                            singleVideoWithNoLanguageCaptions.VideoId,
                            viAccessToken,
                            language, stoppingToken);
                    }
                    catch (Exception retryException)
                    {
                        logger.LogError(exception: retryException,
                            "Retry exception getting captions: {Exceptions}", retryException.Message);
                        return;
                    }
                }
            }
            await dbContext.VideoCaptions.AddAsync(new VideoCaptions()
            {
                Language = language,
                Content = videoCaptions,
                VideoInfoId = singleVideoWithNoLanguageCaptions.VideoInfoId
            }, stoppingToken);
            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
