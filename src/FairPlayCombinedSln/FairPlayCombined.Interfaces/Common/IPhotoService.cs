using FairPlayCombined.Models.Common.Photo;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IPhotoService
    {
        Task UpdatePhotoAsync(UpdatePhotoModel updatePhotoModel, CancellationToken cancellationToken);
        Task<long> CreatePhotoAsync(CreatePhotoModel createModel, CancellationToken cancellationToken);
        Task<PhotoModel[]> GetAllPhotoAsync(CancellationToken cancellationToken);
        Task<PhotoModel> GetPhotoByIdAsync(long id, CancellationToken cancellationToken);
        Task DeletePhotoByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<PhotoModel>> GetPaginatedPhotoAsync(PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
    }
}
