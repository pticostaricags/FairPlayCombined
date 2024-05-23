using FairPlayCombined.Models.FairPlayTube.VideoViewer;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoViewerService
    {
        Task<PaginationOfT<VideoViewerModel>> GetPaginatedVideoViewerInfoForUserIdAsync(
            PaginationRequest paginationRequest, string videoId, string userId, 
            CancellationToken cancellationToken);
    }
}
