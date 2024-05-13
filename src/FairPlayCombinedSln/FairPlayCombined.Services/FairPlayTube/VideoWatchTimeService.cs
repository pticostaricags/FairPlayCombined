using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Models.FairPlayTube.VideoWatchTime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public partial class VideoWatchTimeService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<VideoWatchTimeService> logger) : BaseService
    {
        public async Task CreateVideoWatchTimeAsync(string videoId,
            VideoWatchTimeModel videoWatchTimeModel,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(CreateVideoWatchTimeAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            var videoInfoEntity = await dbContext.VideoInfo
                .TagWith($"TAG: {nameof(CreateVideoWatchTimeAsync)}")
                .SingleAsync(p => p.VideoId == videoId,
                cancellationToken: cancellationToken);
            DateTimeOffset sessionStartDatetime = DateTimeOffset.UtcNow;
            VideoWatchTime? entity = new()
            {
                SessionId = videoWatchTimeModel.SessionId!.Value,
                SessionStartDatetime = sessionStartDatetime,
                LastUpdateDatetime = sessionStartDatetime,
                VideoInfoId = videoInfoEntity.VideoInfoId,
                WatchedByApplicationUserId = videoWatchTimeModel.WatchedByApplicationUserId,
                WatchTime = videoWatchTimeModel.WatchTime
            };
            await dbContext.VideoWatchTime.AddAsync(entity, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }
        public async Task UpdateVideoWatchTimeAsync(
            VideoWatchTimeModel videoWatchTimeModel,
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(UpdateVideoWatchTimeAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            VideoWatchTime? entity = null;
            entity = await dbContext.VideoWatchTime
                .TagWith($"TAG: {nameof(UpdateVideoWatchTimeAsync)}")
                .SingleOrDefaultAsync(p => p.SessionId == videoWatchTimeModel.SessionId,
                cancellationToken: cancellationToken);
            entity!.WatchTime = videoWatchTimeModel.WatchTime;
            entity.LastUpdateDatetime = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }
    }
}
