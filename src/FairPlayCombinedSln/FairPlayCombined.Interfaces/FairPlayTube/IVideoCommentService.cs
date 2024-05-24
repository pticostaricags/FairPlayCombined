using FairPlayCombined.Models.FairPlayTube.VideoComment;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoCommentService
    {
        Task<PaginationOfT<VideoCommentModel>> GetPaginatedCommentsByVideonIdAsync(
            PaginationRequest paginationRequest, string videoId, CancellationToken cancellationToken);
        Task<long> CreateVideoCommentAsync(CreateVideoCommentModel createModel,
            CancellationToken cancellationToken);
        Task<VideoCommentModel[]> GetAllVideoCommentAsync(CancellationToken cancellationToken);
        Task<VideoCommentModel> GetVideoCommentByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteVideoCommentByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoCommentModel>> GetPaginatedVideoCommentAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken);
    }
}
