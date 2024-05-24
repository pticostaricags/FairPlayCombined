namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoDigitalMarketingPlanService
    {
        Task<string?> GetVideoDigitalMarketingPlanAsync(long videoInfoId, string socialNetworkName,
            CancellationToken cancellationToken);
        Task SaveVideoDigitalMarketingPlanAsync(long videoInfoId, string htmlDigitalMarketingPlan,
            string socialNetworkName, bool replaceExistent, CancellationToken cancellationToken);
    }
}
