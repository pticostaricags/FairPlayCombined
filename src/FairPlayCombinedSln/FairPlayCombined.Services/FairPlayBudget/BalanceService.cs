using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayBudget.Balance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayBudget
{
    public class BalanceService(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService)
    {
        public async Task<string[]> GetBudgetNamesAsync(CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext!
                .MonthlyBudgetInfo.Select(p => p.Description).Distinct().ToArrayAsync(
                cancellationToken: cancellationToken);
            return result;
        }

        public async Task<MyBalanceModel[]> GetMyBalanceAsync(
            string budgetName,
            long currencyId,
            CancellationToken cancellationToken)
        {
            var userId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var result = await dbContext.VwBalance
                .Where(p => p.OwnerId == userId && p.MonthlyBudgetDescription == budgetName
                && p.CurrencyId == currencyId
                )
                .Select(p => new MyBalanceModel()
                {
                    Amount = p.Amount,
                    Currency = p.Currency,
                    DateTime = p.DateTime,
                    Description = p.Description,
                    TransactionType = p.TransactionType,
                    MonthlyBudgetDescription = p.MonthlyBudgetDescription
                })
                .ToArrayAsync(cancellationToken: cancellationToken);
            return result;
        }
    }
}
