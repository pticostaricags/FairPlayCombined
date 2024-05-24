using FairPlayCombined.Models.FairPlayTube.VideoPlan;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoPlanService
    {
        Task UpdateVideoPlanAsync(UpdateVideoPlanModel createModel, CancellationToken cancellationToken);
        Task<long> CreateVideoPlanAsync(CreateVideoPlanModel createModel, CancellationToken cancellationToken);
        Task<VideoPlanModel[]> GetAllVideoPlanAsync(CancellationToken cancellationToken);
        Task<VideoPlanModel> GetVideoPlanByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteVideoPlanByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<VideoPlanModel>> GetPaginatedVideoPlanAsync(PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
    }
}
