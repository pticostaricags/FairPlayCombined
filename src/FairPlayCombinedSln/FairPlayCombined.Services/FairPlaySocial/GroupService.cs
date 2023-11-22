using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.Models.FairPlaySocial.Group;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
