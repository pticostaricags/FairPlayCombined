namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IFairPlayTubeMetrics
    {
        public const string SESSION_METER_NAME = $"{nameof(FairPlayTube)}.Videos";
        void Initialize();
    }
}
