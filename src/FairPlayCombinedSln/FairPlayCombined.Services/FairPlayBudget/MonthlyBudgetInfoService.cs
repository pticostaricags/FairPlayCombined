using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayBudget
{
    [ServiceOfT<
        CreateMonthlyBudgetInfoModel,
        UpdateMonthlyBudgetInfoModel,
        MonthlyBudgetInfoModel,
        FairPlayCombinedDbContext,
        MonthlyBudgetInfo,
        PaginationRequest,
        PaginationOfT<MonthlyBudgetInfoModel>
        >]
    public partial class MonthlyBudgetInfoService : BaseService
    {
    }
}
