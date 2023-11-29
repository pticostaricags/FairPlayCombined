using FairPlayCombined.Common.FairPlayTube.Enums;
using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace FairPlayTube.VideoIndexing;

public class VideoIndexStatusService(ILogger<VideoIndexStatusService> logger,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            var scope = serviceScopeFactory.CreateScope();
            var dbContextFactory = scope.ServiceProvider
                .GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            var dbContext = await dbContextFactory.CreateDbContextAsync(stoppingToken);
            var allVideosInProcessingStatus = await dbContext.VideoInfo.
                Where(p => p.VideoIndexStatusId == (short)VideoIndexStatus.Processing)
                .ToArrayAsync(stoppingToken);
            if (allVideosInProcessingStatus.Any()) 
            {
                
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
