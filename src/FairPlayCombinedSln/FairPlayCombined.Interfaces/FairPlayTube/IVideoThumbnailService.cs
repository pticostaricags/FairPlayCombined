using FairPlayCombined.Models.FairPlayTube.VideoThumbnail;
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
    public interface IVideoThumbnailService
    {
        Task<PaginationOfT<VideoThumbnailModel>> 
            GetPaginatedVideoThumbnailByVideoInfoIdAsync(long videoInfoId, PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
        Task<long> CreateVideoThumbnailAsync(CreateVideoThumbnailModel createModel,
            CancellationToken cancellationToken);
        Task<VideoThumbnailModel[]> GetAllVideoThumbnailAsync(CancellationToken cancellationToken);
        Task<VideoThumbnailModel> GetVideoThumbnailByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteVideoThumbnailByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoThumbnailModel>> GetPaginatedVideoThumbnailAsync(
            PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
