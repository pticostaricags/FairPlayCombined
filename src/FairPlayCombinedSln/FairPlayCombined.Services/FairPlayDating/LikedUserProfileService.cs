using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.LikedUserProfile;
using FairPlayCombined.Models.Pagination;

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
