namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoDigitalMarketingDailyPostsService
    {
        Task<string?> GetVideoDigitalMarketingDailyPostsAsync(long videoInfoId, string socialNetworkName,
            CancellationToken cancellationToken);
        Task SaveVideoDigitalMarketingDailyPostsAsync(long videoInfoId,
            string htmlVideoDigitalMarketingDailyPostsIdeas, string socialNetworkName, 
            CancellationToken cancellationToken);
    }
}