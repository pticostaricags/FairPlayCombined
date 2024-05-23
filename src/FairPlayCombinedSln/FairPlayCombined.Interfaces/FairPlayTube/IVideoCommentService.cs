using FairPlayCombined.Models.FairPlayTube.VideoComment;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoCommentService
    {
        Task<PaginationOfT<VideoCommentModel>> GetPaginatedCommentsByVideonIdAsync(
            PaginationRequest paginationRequest, string videoId,CancellationToken cancellationToken);
        Task<long> CreateVideoCommentAsync(CreateVideoCommentModel createModel,
            CancellationToken cancellationToken);
        Task<VideoCommentModel[]> GetAllVideoCommentAsync(CancellationToken cancellationToken);
        Task<VideoCommentModel> GetVideoCommentByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteVideoCommentByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoCommentModel>> GetPaginatedVideoCommentAsync(PaginationRequest paginationRequest,
        CancellationToken cancellationToken);
    }
}
