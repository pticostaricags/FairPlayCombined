using FairPlayCombined.Models.Common.AspNetUsers;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IAspNetUsersService
    {
        Task<string> CreateAspNetUsersAsync(CreateAspNetUsersModel createModel,
            CancellationToken cancellationToken);
        Task<AspNetUsersModel[]> GetAllAspNetUsersAsync(CancellationToken cancellationToken);
        Task<AspNetUsersModel> GetAspNetUsersByIdAsync(string id, CancellationToken cancellationToken);
        Task DeleteAspNetUsersByIdAsync(string id, CancellationToken cancellationToken);
        Task<PaginationOfT<AspNetUsersModel>> GetPaginatedAspNetUsersAsync(PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
    }
}
