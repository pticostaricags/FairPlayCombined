using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayDating
{
    [ServiceOfT<
        CreateActivityModel,
        UpdateActivityModel,
        ActivityModel,
        FairPlayCombinedDbContext,
        Activity,
        PaginationRequest,
        PaginationOfT<ActivityModel>>
        ]
    public partial class ActivityService
    {
    }
}
