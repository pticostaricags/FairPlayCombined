using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.LikedUserProfile;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateLikedUserProfileModel,
        UpdateLikedUserProfileModel,
        LikedUserProfileModel,
        FairPlayCombinedDbContext,
        LikedUserProfile,
        PaginationRequest,
        PaginationOfT<LikedUserProfileModel>
        >]
    public partial class LikedUserProfileService : BaseService
    {
    }
}
