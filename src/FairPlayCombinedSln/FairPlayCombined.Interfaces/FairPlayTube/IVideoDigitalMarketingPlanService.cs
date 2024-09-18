namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoDigitalMarketingPlanService
    {
        Task<string[]?> GetVideoDigitalMarketingPlansAsync(long videoInfoId, string socialNetworkName,
            CancellationToken cancellationToken);
        Task SaveVideoDigitalMarketingPlanAsync(long videoInfoId, string htmlDigitalMarketingPlan,
            string socialNetworkName, bool replaceExistent, CancellationToken cancellationToken);
        Task<string> CreateVideoDigitalMarketingPlanAsync(long videoInfoId, CancellationToken cancellationToken);
    }
}
