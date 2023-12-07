using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.Models.FairPlayBudget.Currency;
using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayBudget
{
    [ServiceOfT<
        CreateCurrencyModel,
        UpdateCurrencyModel,
        CurrencyModel,
        FairPlayCombinedDbContext,
        Currency,
        PaginationRequest,
        PaginationOfT<CurrencyModel>
        >]
    public partial class CurrencyService : BaseService
    {
    }
}
