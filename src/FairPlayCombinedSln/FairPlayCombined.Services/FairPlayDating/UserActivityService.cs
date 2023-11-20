using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.UserActivity;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
