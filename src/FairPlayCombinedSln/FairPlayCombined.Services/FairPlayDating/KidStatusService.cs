using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.KidStatus;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateKidStatusModel,
        UpdateKidStatusModel,
        KidStatusModel,
        FairPlayCombinedDbContext,
        KidStatus,
        PaginationRequest,
        PaginationOfT<KidStatusModel>
        >]
    public partial class KidStatusService : BaseService
    {
    }
}
