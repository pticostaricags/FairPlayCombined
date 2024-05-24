using FairPlayCombined.Models.FairPlayTube.VideoViewer;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoViewerService
    {
        Task<PaginationOfT<VideoViewerModel>> GetPaginatedVideoViewerInfoForUserIdAsync(
            PaginationRequest paginationRequest, string videoId, string userId,
            CancellationToken cancellationToken);
    }
}
