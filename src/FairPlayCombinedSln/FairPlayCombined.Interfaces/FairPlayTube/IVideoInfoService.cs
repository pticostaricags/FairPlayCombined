using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using System.Threading;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoInfoService
    {
        Task CreateDescriptionForVideoAsync(long videoInfoId, CancellationToken cancellationToken);
        Task<long> CreateVideoInfoAsync(CreateVideoInfoModel createModel,
            CancellationToken cancellationToken);
        Task<VideoInfoModel[]> GetAllVideoInfoAsync(CancellationToken cancellationToken);
        Task<VideoInfoModel> GetVideoInfoByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteVideoInfoByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoInfoModel>> GetPaginatedVideoInfoAsync(
            PaginationRequest paginationRequest, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoInfoModel>> GetPaginatedCompletedVideoInfobyUserIdAsync(
            PaginationRequest paginationRequest,
            string userId,
            CancellationToken cancellationToken);

        Task<PaginationOfT<VideoInfoModel>> GetPaginatedNotCompletedVideoInfobyUserIdAsync(
            PaginationRequest paginationRequest,
            string userId,
            CancellationToken cancellationToken);
        Task<PaginationOfT<VideoInfoModel>> GetPaginatedCompletedVideoInfoAsync(
            PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
        Task<VideoInfoModel?> GetVideoInfoByVideoIdAsync(string videoId,
            CancellationToken cancellationToken);
        Task DeleteMyVideoAsync(long videoInfoId, CancellationToken cancellationToken);
    }
}
