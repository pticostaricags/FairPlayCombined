using FairPlayCombined.Common.FairPlayTube.Enums;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace FairPlayTube.VideoIndexing;

public class VideoIndexStatusService(ILogger<VideoIndexStatusService> logger,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
            var allVideosInProcessingStatus = await dbContext.VideoInfo.
                Where(p => p.VideoIndexStatusId == 
                (short)FairPlayCombined.Common.FairPlayTube.Enums.VideoIndexStatus.Processing)
                .Select(p => p.VideoId)
                .ToArrayAsync(stoppingToken);
            if (allVideosInProcessingStatus.Any())
            {
                var armAccessToken = await azureVideoIndexerService.AuthenticateToAzureArmAsync();
                var getviTokenResult = await azureVideoIndexerService.GetAccessTokenForArmAccountAsync(armAccessToken, stoppingToken);
                var videosIndex = await azureVideoIndexerService.SearchVideosByIdsAsync(
                    getviTokenResult!.AccessToken!, allVideosInProcessingStatus, stoppingToken);
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
                    }

                    await dbContext.SaveChangesAsync(cancellationToken: stoppingToken);
                }
            }
            await Task.Delay(1000, stoppingToken);
        }
    }
}
