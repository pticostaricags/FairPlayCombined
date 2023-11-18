using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Models.Role;

namespace FairPlayCombined.Interfaces
{
    public interface IRoleService
    {
        Task<PaginationOfT<RoleModel>> GetPaginatedRoleListAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
