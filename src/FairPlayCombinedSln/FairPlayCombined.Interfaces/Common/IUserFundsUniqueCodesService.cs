using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Models.Common.UserFundsUniqueCodes;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IUserFundsUniqueCodesService
    {
        Task ClaimFundsUniqueCodeAsync(Guid code, CancellationToken cancellationToken);
        Task<Guid> CreateFundsUniqueCodeAsync(CreateUserFundsUniqueCodesModel createUserFundsUniqueCodesModel, CancellationToken cancellationToken);
        Task<PaginationOfT<UserFundsUniqueCodesModel>> GetPaginatedUserFundsUniqueCodesListAsync(PaginationRequest paginationRequest, CancellationToken cancellationToken);
    }
}
