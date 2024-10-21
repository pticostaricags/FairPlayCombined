using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.FairPlayTube;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace FairPlayTube.Metrics
{
    public class FairPlayTubeMetrics(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory)
        : IFairPlayTubeMetrics
    {
        private Meter? SessionsMeter { get; set; }

        public void Initialize()
        {
            SessionsMeter = new(IFairPlayTubeMetrics.SESSION_METER_NAME);
            SessionsMeter!.CreateObservableGauge($"{SessionsMeter!.Name}.RealtimeVideoSessions",
                observeValue: () =>
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime
                    .Where(p => p.LastUpdateDatetime >=
                    DateTimeOffset.UtcNow.AddSeconds(-10))
                    .Count();
                    return result;
                });
            SessionsMeter!.CreateObservableGauge($"{SessionsMeter!.Name}.RealtimeAuthenticatedVideoSessions",
                observeValue: () =>
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime
                    .Where(p => p.LastUpdateDatetime >=
                    DateTimeOffset.UtcNow.AddSeconds(-10) &&
                    p.WatchedByApplicationUserId != null)
                    .Count();
                    return result;
                });
            SessionsMeter!.CreateObservableGauge($"{SessionsMeter!.Name}.LifetimeAuthenticatedVideoSessions",
                observeValue: () =>
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime.Count();
                    return result;
                });
        }
    }
}