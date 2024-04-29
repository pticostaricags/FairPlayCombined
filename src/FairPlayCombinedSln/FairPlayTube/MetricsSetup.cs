using System.Diagnostics.Metrics;


namespace FairPlayTube.MetricsConfiguration
{
    public class MetricsSetup
    {
        internal const string SESSION_METER_NAME = $"{nameof(FairPlayTube)}.Videos";
        private Meter? sessionsMeter { get; set; }
        private Counter<int>? videoSessions { get; set; }

        public MetricsSetup()
        {
            sessionsMeter = new(SESSION_METER_NAME);
            videoSessions = sessionsMeter!.CreateCounter<int>($"{sessionsMeter!.Name}.VideoSessions");
        }

        public void AddSession()
        {
            this.videoSessions!.Add(1);
        }
    }
}