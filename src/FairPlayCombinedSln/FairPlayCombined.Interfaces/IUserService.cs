using FairPlayCombined.Models;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Interfaces
{
    public interface IUserService
    {
        Task<PaginationOfT<UserModel>> GetPaginatedUserListAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
