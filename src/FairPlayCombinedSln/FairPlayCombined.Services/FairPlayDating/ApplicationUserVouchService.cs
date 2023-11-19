using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating;
using FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
