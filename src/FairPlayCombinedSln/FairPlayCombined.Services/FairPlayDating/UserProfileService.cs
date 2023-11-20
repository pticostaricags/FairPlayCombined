using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.UserProfile;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateUserProfileModel,
        UpdateUserProfileModel,
        UserProfileModel,
        FairPlayCombinedDbContext,
        UserProfile,
        PaginationRequest,
        PaginationOfT<UserProfileModel>
        >]
    public partial class UserProfileService : BaseService
    {
    }
}
