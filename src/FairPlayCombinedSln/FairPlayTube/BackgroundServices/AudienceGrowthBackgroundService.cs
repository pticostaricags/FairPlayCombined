
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Text;

namespace FairPlayTube.BackgroundServices;

public class AudienceGrowthBackgroundService(IServiceProvider serviceProvider,
     ILogger<AudienceGrowthBackgroundService> logger) : BackgroundService
{
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
    {
        var timeToWait = TimeSpan.FromHours(1);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
                var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
                foreach (var singleLinkedInClaimUserId in dbContext.AspNetUserTokens.Where(p =>
                p.LoginProvider == "LinkedIn" &&
                p.Name == "access_token")
                    .AsNoTracking().Select(p => p.UserId))
                {
                    var currentTime = DateTimeOffset.UtcNow;
                    DateTimeOffset timeToStart = DateTimeOffset.UtcNow;
                    var lastPublishedItemForUser = await dbContext.VideoAudienceGrowthQueue
                        .AsNoTracking()
                        .Where(p => p.VideoInfo.ApplicationUserId == singleLinkedInClaimUserId)
                        .OrderByDescending(p => p.LastTimePublished)
                        .FirstOrDefaultAsync(stoppingToken);
                    int hoursToAdd = 6;
                    if (lastPublishedItemForUser != null)
                        timeToStart = lastPublishedItemForUser.LastTimePublished.AddHours(hoursToAdd);
                    foreach (var singleUserVideo in
                        dbContext.VideoInfo.Include(p => p.VideoAudienceGrowthQueue)
                        .Where(p => p.ApplicationUserId == singleLinkedInClaimUserId)
                        .AsNoTracking())
                    {
                        try
                        {
                            if (singleUserVideo.VideoAudienceGrowthQueue == null ||
                                (currentTime > singleUserVideo.VideoAudienceGrowthQueue.LastTimePublished.AddMonths(1)))
                            {
                                BackgroundJob.Schedule<AudienceGrowthJob>(p => p.Execute(singleUserVideo.VideoInfoId), timeToStart);
                                var entity = await dbContext
                                    .VideoAudienceGrowthQueue
                                    .SingleOrDefaultAsync(
                                    p=>p.VideoInfoId == 
                                    singleUserVideo.VideoInfoId,stoppingToken);
                                if (entity is null)
                                {
                                    entity = new()
                                    {
                                        LastTimePublished = timeToStart,
                                        VideoInfoId = singleUserVideo.VideoInfoId
                                    };
                                    await dbContext.VideoAudienceGrowthQueue.AddAsync(entity, stoppingToken);
                                }
                                else
                                {
                                    entity.LastTimePublished = timeToStart;
                                }

                                await dbContext.SaveChangesAsync(stoppingToken);
                                timeToStart = timeToStart.AddHours(hoursToAdd);
                            }
                        }
                        catch (Exception scheduleEx)
                        {
                            logger.LogError(scheduleEx, "An error has occurred: {ErrorMessage}", scheduleEx.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, message: "An error has occurred: {ErrorMessage}", ex.Message);
            }
            await Task.Delay(timeToWait);
        }
    }
}

public class AudienceGrowthJob(IServiceProvider serviceProvider,
    ILogger<AudienceGrowthJob> logger)
{
    public async Task Execute(long videoInfoId)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var localizer = scope.ServiceProvider.GetRequiredService<IStringLocalizer<AudienceGrowthJob>>();
            var linkedInClientService = scope.ServiceProvider.GetRequiredService<ILinkedInClientService>();
            var dbContextFactory =
                scope.ServiceProvider.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            var dbContext = dbContextFactory.CreateDbContext();
            var videoData = await dbContext.VideoInfo
                .SingleAsync(p => p.VideoInfoId == videoInfoId);
            var thumbnailEntity = await dbContext.VideoThumbnail.Include(p => p.Photo).FirstOrDefaultAsync(p => p.VideoInfoId == videoInfoId);
            if (thumbnailEntity is not null)
            {
                StringBuilder stringBuilder = new(videoData.Description);
                stringBuilder.Append($" Visit https://fairplaytube.pticostarica.com/Public/WatchVideo/{videoData.VideoId}");
                stringBuilder.Append($" {localizer[DisclaimerTextKey]}");
                MemoryStream memoryStream = new(thumbnailEntity.Photo.PhotoBytes);
                var accessToken = await linkedInClientService.GetAccessTokenForUserAsync(videoData.ApplicationUserId, CancellationToken.None);
                await linkedInClientService.CreateImageShareAsync(stringBuilder.ToString(),
                    accessToken, memoryStream, filename: videoData.Name,
                    mediaDescription: videoData.Description,
                    mediaTitle: videoData.Name, cancellationToken: CancellationToken.None);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error ocurred: {ErrorMessage}", ex.Message);
        }
    }

    #region Resource Keys
    [ResourceKey(defaultValue: "DISCLAIMER: Text and images were generated using AI")]
    public const string DisclaimerTextKey = "DisclaimerText";
    #endregion Resource Keys
}