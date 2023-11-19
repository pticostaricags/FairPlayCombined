using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.DateObjective;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateDateObjectiveModel,
        UpdateDateObjectiveModel,
        DateObjectiveModel,
        FairPlayCombinedDbContext,
        DateObjective,
        PaginationRequest,
        PaginationOfT<DateObjectiveModel>
        >]
    public partial class DateObjectiveService : BaseService
    {
    }
}
