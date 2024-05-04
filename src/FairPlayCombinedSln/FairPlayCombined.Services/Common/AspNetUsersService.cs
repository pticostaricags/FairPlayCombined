using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.Common.AspNetUsers;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.Common
{
    [ServiceOfT<
        CreateAspNetUsersModel,
        UpdateAspNetUsersModel,
        AspNetUsersModel,
        FairPlayCombinedDbContext,
        AspNetUsers,
        PaginationRequest,
        PaginationOfT<AspNetUsersModel>
        >]
    public partial class AspNetUsersService : BaseService
    {
    }
}
