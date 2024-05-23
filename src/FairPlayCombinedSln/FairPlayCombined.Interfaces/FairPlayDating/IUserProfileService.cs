using FairPlayCombined.Models.FairPlayDating.UserProfile;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayDating
{
    public interface IUserProfileService
    {
        Task<long> CreateUserProfileExtendedAsync(CreateUserProfileModel createModel,
            CancellationToken cancellationToken);
        Task<long?> GetUserProfileIdByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<UserProfileModel?> GetUserProfileByUserIdAsync(string userId, CancellationToken cancellationToken);
        Task<long> CreateUserProfileAsync(CreateUserProfileModel createModel,
            CancellationToken cancellationToken);
        Task<UserProfileModel[]> GetAllUserProfileAsync(CancellationToken cancellationToken);
        Task<UserProfileModel> GetUserProfileByIdAsync(long id, CancellationToken cancellationToken);
        Task DeleteUserProfileByIdAsync(long id, CancellationToken cancellationToken);
        Task<PaginationOfT<UserProfileModel>> GetPaginatedUserProfileAsync(PaginationRequest paginationRequest,
            CancellationToken cancellationToken);
    }
}
