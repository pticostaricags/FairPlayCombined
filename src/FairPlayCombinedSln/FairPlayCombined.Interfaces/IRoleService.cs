using FairPlayCombined.Models;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces
{
    public interface IRoleService
    {
        Task<PaginationOfT<RoleModel>> GetPaginatedRoleListAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
