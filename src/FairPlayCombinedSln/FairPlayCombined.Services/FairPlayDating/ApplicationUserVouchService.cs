using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch;
using FairPlayCombined.Models.Pagination;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateApplicationUserVouchModel,
        UpdateApplicationUserVouchModel,
        ApplicationUserVouchModel,
        FairPlayCombinedDbContext,
        ApplicationUserVouch,
        PaginationRequest,
        PaginationOfT<ApplicationUserVouchModel>>
        ]
    public partial class ApplicationUserVouchService : BaseService
    {
    }
}
