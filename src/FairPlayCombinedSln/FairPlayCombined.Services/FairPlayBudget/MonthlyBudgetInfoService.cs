using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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
        public async Task<PaginationOfT<MonthlyBudgetInfoModel>> GetPaginatedMonthlyBudgetInfoForUserIdAsync(
            string userId,
    PaginationRequest paginationRequest,
    CancellationToken cancellationToken
    )
        {
            PaginationOfT<MonthlyBudgetInfoModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.MonthlyBudgetInfo
                .Where(p=>p.OwnerId == userId)
                .Select(p => new FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo.MonthlyBudgetInfoModel
                {
                    MonthlyBudgetInfoId = p.MonthlyBudgetInfoId,
                    Description = p.Description,
                    OwnerId = p.OwnerId,

                });
            if (!String.IsNullOrEmpty(orderByString))
                query = query.OrderBy(orderByString);
            result.TotalItems = await query.CountAsync(cancellationToken);
            result.PageSize = paginationRequest.PageSize;
            result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
            result.Items = await query
            .Skip(paginationRequest.StartIndex)
            .Take(paginationRequest.PageSize)
            .ToArrayAsync(cancellationToken);
            return result;
        }
    }
}
