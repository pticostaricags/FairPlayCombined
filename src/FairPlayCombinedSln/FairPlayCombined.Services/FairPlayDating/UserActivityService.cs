using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.UserActivity;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateUserActivityModel,
        UpdateUserActivityModel,
        UserActivityModel,
        FairPlayCombinedDbContext,
        UserActivity,
        PaginationRequest,
        PaginationOfT<UserActivityModel>
        >]
    public partial class UserActivityService : BaseService
    {
    }
}
