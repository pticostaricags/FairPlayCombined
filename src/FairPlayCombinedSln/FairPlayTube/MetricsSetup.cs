using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;


namespace FairPlayTube.MetricsConfiguration
{
    public class MetricsSetup(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory)
    {
        internal const string SESSION_METER_NAME = $"{nameof(FairPlayTube)}.Videos";
        private Meter? sessionsMeter { get; set; }
        private ObservableGauge<int>? videoSessions { get; set; }

        public void Initialize()
        {
            sessionsMeter = new(SESSION_METER_NAME);
            videoSessions = sessionsMeter!.CreateObservableGauge<int>($"{sessionsMeter!.Name}.VideoSessions",
                observeValue: () => 
                {
                    var dbContext = dbContextFactory.CreateDbContext();
                    var result = dbContext.VideoWatchTime
                    .Where(p=>p.LastUpdateDatetime >= 
                    DateTimeOffset.UtcNow.AddSeconds(-10))
                    .Count();
                    return result;
                });
        }
    }
}