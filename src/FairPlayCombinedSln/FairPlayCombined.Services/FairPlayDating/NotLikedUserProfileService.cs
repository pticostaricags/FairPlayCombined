using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.NotLikedUserProfile;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateNotLikedUserProfileModel,
        UpdateNotLikedUserProfileModel,
        NotLikedUserProfileModel,
        FairPlayCombinedDbContext,
        NotLikedUserProfile,
        PaginationRequest,
        PaginationOfT<NotLikedUserProfileModel>
        >]
    public partial class NotLikedUserProfileService : BaseService
    {
    }
}
