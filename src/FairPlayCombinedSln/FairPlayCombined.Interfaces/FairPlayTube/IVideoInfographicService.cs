using FairPlayCombined.Models.FairPlayTube.VideoInfographic;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoInfographicService
    {
        Task<PaginationOfT<VideoInfographicModel>>
            GetPaginatedVideoInfographicByVideoInfoIdAsync(long videoInfoId, PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
        Task<long> CreateVideoInfographicAsync(CreateVideoInfographicModel createModel,
            CancellationToken cancellationToken);
        Task<VideoInfographicModel[]> GetAllVideoInfographicAsync(CancellationToken cancellationToken);
        Task<VideoInfographicModel> GetVideoInfographicByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteVideoInfographicByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoInfographicModel>> GetPaginatedVideoInfographicAsync(
            PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
