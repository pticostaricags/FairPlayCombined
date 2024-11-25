
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
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
                ISchedulerFactory schedulerFactory = scope.ServiceProvider.GetRequiredService<ISchedulerFactory>();
                var scheduler = await schedulerFactory.GetScheduler(stoppingToken);
                foreach (var singleLinkedInClaim in dbContext.AspNetUserTokens.Where(p =>
                p.LoginProvider == "LinkedIn" &&
                p.Name == "access_token"))
                {
                    DateTimeOffset timeToStart = DateTimeOffset.UtcNow;
                    foreach (var singleUserVideo in 
                        dbContext.VideoInfo.Where(p => p.ApplicationUserId == singleLinkedInClaim.UserId)
                        .OrderByDescending(p=>p.VideoInfoId))
                    {
                        string jobName = $"{singleUserVideo.ApplicationUserId}-{singleUserVideo.VideoInfoId}";
                        JobKey jobKey = new(jobName);
                        if (!await scheduler.CheckExists(jobKey))
                        {
                            try
                            {
                                var job = JobBuilder.Create<AudienceGrowthJob>()
                                    .WithIdentity(jobName)
                                    .Build();
                                var trigger = TriggerBuilder.Create()
                                    .WithIdentity($"tr-{singleUserVideo.ApplicationUserId}-{singleUserVideo.VideoInfoId}")
                                    .StartAt(timeToStart)
                                    .UsingJobData("VideoInfoId", singleUserVideo.VideoInfoId.ToString())
                                    .Build();
                                await scheduler.ScheduleJob(job, trigger, stoppingToken);
                                timeToStart = timeToStart.AddHours(3);
                            }
                            catch (Exception jobAndTriggerEx)
                            {
                                logger.LogError(jobAndTriggerEx, "An error ocurred: {ErrorMessage}", jobAndTriggerEx.Message);
                            }
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
    ILogger<AudienceGrowthJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            using var scope = serviceProvider.CreateScope();
            var linkedInClientService = scope.ServiceProvider.GetRequiredService<ILinkedInClientService>();
            var dbContextFactory =
                scope.ServiceProvider.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            var dbContext = dbContextFactory.CreateDbContext();
            var videoInfoIdString = context.MergedJobDataMap.Single(p => p.Key == "VideoInfoId").Value;

            long videoInfoId = Convert.ToInt64(videoInfoIdString);
            var videoData = await dbContext.VideoInfo
                .SingleAsync(p => p.VideoInfoId == videoInfoId);
            var thumbnailEntity = await dbContext.VideoThumbnail.Include(p => p.Photo).FirstOrDefaultAsync(p => p.VideoInfoId == videoInfoId);
            if (thumbnailEntity is not null)
            {
                StringBuilder stringBuilder = new(videoData.Description);
                stringBuilder.Append($" Visit https://fairplaytube.pticostarica.com/Public/WatchVideo/{videoData.VideoId}");
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
}