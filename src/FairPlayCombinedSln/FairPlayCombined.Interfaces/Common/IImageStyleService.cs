using FairPlayCombined.Models.Common.ImageStyle;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IImageStyleService
    {
        Task<int> CreateImageStyleAsync(CreateImageStyleModel createModel,
            CancellationToken cancellationToken);

        Task<ImageStyleModel[]> GetAllImageStyleAsync(CancellationToken cancellationToken);

        Task<ImageStyleModel> GetImageStyleByIdAsync(
        int id, CancellationToken cancellationToken);

        Task DeleteImageStyleByIdAsync(
        int id, CancellationToken cancellationToken);

        Task<PaginationOfT<ImageStyleModel>> GetPaginatedImageStyleAsync(
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken);
    }
}
