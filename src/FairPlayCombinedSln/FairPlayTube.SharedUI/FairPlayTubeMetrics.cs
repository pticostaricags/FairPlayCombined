using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace FairPlayTube.MetricsConfiguration
{
    public class FairPlayTubeMetrics(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory)
    {
        public const string SESSION_METER_NAME = $"{nameof(FairPlayTube)}.Videos";
        private Meter? SessionsMeter { get; set; }
        private ObservableGauge<int>? RealtimeVideoSessions { get; set; }
        private ObservableGauge<int>? RealtimeAuthenticatedVideoSessions { get; set; }
        private ObservableGauge<int>? LifetimeVideoSessions { get; set; }

        public void Initialize()
        {
            SessionsMeter = new(SESSION_METER_NAME);
            RealtimeVideoSessions = SessionsMeter!.CreateObservableGauge<int>($"{SessionsMeter!.Name}.RealtimeVideoSessions",
                observeValue: () => 
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime
                    .Where(p=>p.LastUpdateDatetime >= 
                    DateTimeOffset.UtcNow.AddSeconds(-10))
                    .Count();
                    return result;
                });
            RealtimeAuthenticatedVideoSessions = SessionsMeter!.CreateObservableGauge<int>($"{SessionsMeter!.Name}.RealtimeAuthenticatedVideoSessions",
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
            LifetimeVideoSessions = SessionsMeter!.CreateObservableGauge<int>($"{SessionsMeter!.Name}.LifetimeAuthenticatedVideoSessions",
                observeValue: () =>
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime.Count();
                    return result;
                });
        }
    }
}