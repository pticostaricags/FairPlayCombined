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
            string userId, PaginationRequest paginationRequest,
            CancellationToken cancellationToken)
        {
            PaginationOfT<MonthlyBudgetInfoModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.MonthlyBudgetInfo
                .Where(p => p.OwnerId == userId)
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

        public async Task CreateMonthlyBudgetInfoWithTransactionsAsync(
            CreateMonthlyBudgetInfoModel createMonthlyBudgetInfoModel,
            CancellationToken cancellationToken)
        {
            MonthlyBudgetInfo entity = new()
            {
                Description = createMonthlyBudgetInfoModel.Description,
                OwnerId = createMonthlyBudgetInfoModel.OwnerId
            };
            if (createMonthlyBudgetInfoModel.Transactions?.Count > 0)
            {
                foreach (var singleTransaction in createMonthlyBudgetInfoModel.Transactions)
                {
                    switch (singleTransaction.TransactionType)
                    {
                        case TransactionType.Debit:
                            entity.Expense.Add(new Expense()
                            {
                                Amount = singleTransaction.Amount!.Value,
                                Description = singleTransaction.Description,
                                ExpenseDateTime = singleTransaction.TransactionDateTime!.Value,
                                OwnerId = createMonthlyBudgetInfoModel.OwnerId,
                                CurrencyId = singleTransaction.CurrencyId!.Value
                            });
                            break;
                        case TransactionType.Credit:
                            entity.Income.Add(new Income()
                            {
                                Amount = singleTransaction.Amount!.Value,
                                Description = singleTransaction.Description,
                                IncomeDateTime = singleTransaction.TransactionDateTime!.Value,
                                OwnerId = createMonthlyBudgetInfoModel.OwnerId,
                                CurrencyId = singleTransaction.CurrencyId!.Value
                            });
                            break;
                    }
                }
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                await dbContext.MonthlyBudgetInfo
                    .AddAsync(entity, cancellationToken: cancellationToken);
                await dbContext.SaveChangesAsync(
                    cancellationToken: cancellationToken);
            }
        }

        public async Task UpdateMonthlyBudgetInfoWithTransactionsAsync(long monthlyBudgetInfoModelId, 
            CreateMonthlyBudgetInfoModel createMonthlyBudgetInfoModel, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var monthlyInfoEntity = await dbContext.MonthlyBudgetInfo
                .Include(p => p.Expense)
                .Include(p => p.Income)
                .SingleOrDefaultAsync(p => p.MonthlyBudgetInfoId == monthlyBudgetInfoModelId, cancellationToken);
            if (monthlyInfoEntity != null)
            {
                dbContext.Expense.RemoveRange(monthlyInfoEntity.Expense);
                dbContext.Income.RemoveRange(monthlyInfoEntity.Income);
                dbContext.MonthlyBudgetInfo.Remove(monthlyInfoEntity);
                await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            MonthlyBudgetInfo entity = new()
            {
                Description = createMonthlyBudgetInfoModel.Description,
                OwnerId = createMonthlyBudgetInfoModel.OwnerId
            };
            if (createMonthlyBudgetInfoModel.Transactions?.Count > 0)
            {
                foreach (var singleTransaction in createMonthlyBudgetInfoModel.Transactions)
                {
                    switch (singleTransaction.TransactionType)
                    {
                        case TransactionType.Debit:
                            entity.Expense.Add(new Expense()
                            {
                                Amount = singleTransaction.Amount!.Value,
                                Description = singleTransaction.Description,
                                ExpenseDateTime = singleTransaction.TransactionDateTime!.Value,
                                OwnerId = createMonthlyBudgetInfoModel.OwnerId,
                                CurrencyId = singleTransaction.CurrencyId!.Value
                            });
                            break;
                        case TransactionType.Credit:
                            entity.Income.Add(new Income()
                            {
                                Amount = singleTransaction.Amount!.Value,
                                Description = singleTransaction.Description,
                                IncomeDateTime = singleTransaction.TransactionDateTime!.Value,
                                OwnerId = createMonthlyBudgetInfoModel.OwnerId,
                                CurrencyId = singleTransaction.CurrencyId!.Value
                            });
                            break;
                    }
                }
                await dbContext.MonthlyBudgetInfo
                    .AddAsync(entity, cancellationToken: cancellationToken);
            }
            await dbContext.SaveChangesAsync(
                    cancellationToken: cancellationToken);
        }
    }
}
