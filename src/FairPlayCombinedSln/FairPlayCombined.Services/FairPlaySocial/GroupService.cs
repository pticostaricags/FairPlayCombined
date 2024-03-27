using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.Models.FairPlaySocial.Group;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlaySocial
{
    [ServiceOfT<
        CreateGroupModel,
        UpdateGroupModel,
        GroupModel,
        FairPlayCombinedDbContext,
        Group,
        PaginationRequest,
        PaginationOfT<GroupModel>
        >]
    public partial class GroupService : BaseService
    {
    }
}
