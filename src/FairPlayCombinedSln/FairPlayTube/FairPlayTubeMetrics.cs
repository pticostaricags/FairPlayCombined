using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace FairPlayTube.MetricsConfiguration
{
    public class FairPlayTubeMetrics(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory)
    {
        internal const string SESSION_METER_NAME = $"{nameof(FairPlayTube)}.Videos";
        private Meter? sessionsMeter { get; set; }
        private ObservableGauge<int>? realtimeVideoSessions { get; set; }
        private ObservableGauge<int>? realtimeAuthenticatedVideoSessions { get; set; }
        private ObservableGauge<int>? lifetimeVideoSessions { get; set; }

        public void Initialize()
        {
            sessionsMeter = new(SESSION_METER_NAME);
            realtimeVideoSessions = sessionsMeter!.CreateObservableGauge<int>($"{sessionsMeter!.Name}.RealtimeVideoSessions",
                observeValue: () => 
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime
                    .Where(p=>p.LastUpdateDatetime >= 
                    DateTimeOffset.UtcNow.AddSeconds(-10))
                    .Count();
                    return result;
                });
            realtimeAuthenticatedVideoSessions = sessionsMeter!.CreateObservableGauge<int>($"{sessionsMeter!.Name}.RealtimeAuthenticatedVideoSessions",
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
            lifetimeVideoSessions = sessionsMeter!.CreateObservableGauge<int>($"{sessionsMeter!.Name}.LifetimeAuthenticatedVideoSessions",
                observeValue: () =>
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime.Count();
                    return result;
                });
        }
    }
}