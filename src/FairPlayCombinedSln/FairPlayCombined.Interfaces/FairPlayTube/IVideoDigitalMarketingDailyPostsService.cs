using FairPlayCombined.Models.FairPlayTube.VideoDigitalMarketingDailyPosts;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoDigitalMarketingDailyPostsService
    {
        Task<string> CreateVideoDigitalMarketingDailyPostsForTwitterAsync
            (long videoInfoId, string languageCode, CancellationToken cancellationToken);
        Task<string> CreateVideoDigitalMarketingDailyPostsForLinkedInAsync(
            long videoInfoId, string languageCode, CancellationToken cancellationToken);
        Task<string?> GetVideoDigitalMarketingDailyPostsAsync(long videoInfoId, string socialNetworkName,
            CancellationToken cancellationToken);
        Task SaveVideoDigitalMarketingDailyPostsAsync(long videoInfoId,
            string htmlVideoDigitalMarketingDailyPostsIdeas, string socialNetworkName,
            CancellationToken cancellationToken);
        Task<long> CreateVideoDigitalMarketingDailyPostsAsync(
            CreateVideoDigitalMarketingDailyPostsModel createModel,
            CancellationToken cancellationToken);
        Task<VideoDigitalMarketingDailyPostsModel[]> GetAllVideoDigitalMarketingDailyPostsAsync(
            CancellationToken cancellationToken);
        Task<VideoDigitalMarketingDailyPostsModel> GetVideoDigitalMarketingDailyPostsByIdAsync(
            long id, CancellationToken cancellationToken);
        Task DeleteVideoDigitalMarketingDailyPostsByIdAsync(long id,
            CancellationToken cancellationToken);
        Task<PaginationOfT<VideoDigitalMarketingDailyPostsModel>> GetPaginatedVideoDigitalMarketingDailyPostsAsync(
            PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoDigitalMarketingDailyPostsModel>> GetPaginatedVideoDigitalMarketingDailyPostsByVideoInfoIdAsync(
            long videoInfoId, PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}