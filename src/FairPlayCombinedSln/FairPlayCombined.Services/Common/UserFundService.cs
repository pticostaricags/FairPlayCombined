using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using Microsoft.EntityFrameworkCore;

namespace FairPlayCombined.Services.Common
{
    public partial class UserFundService(IPayPalOrderService payPalOrderService,
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService) : IUserFundService
    {
        public async Task<decimal> GetMyAvailableFundsAsync(CancellationToken cancellationToken)
        {
            decimal result = 0;
            var currentUserId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var userFundsEntity = await dbContext.UserFunds
                .SingleOrDefaultAsync(p => p.ApplicationUserId == currentUserId,
                cancellationToken);
            if (userFundsEntity != null)
                result = userFundsEntity.AvailableFunds;
            return result;
        }
        public async Task AddMyFundsAsync(string orderId, CancellationToken cancellationToken)
        {
            var accessToken = await payPalOrderService.GetAccessTokenAsync(cancellationToken);
            var currentUserId = userProviderService.GetCurrentUserId();
            var orderDetails = await payPalOrderService
                .GetOrderDetailsAsync(orderId, accessToken.access_token!, cancellationToken);
            decimal fundsToAdd = Convert.ToDecimal(orderDetails!.purchase_units![0].items![0].price);
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var userFundsEntity = await dbContext.UserFunds
                .SingleOrDefaultAsync(p => p.ApplicationUserId == currentUserId,
                cancellationToken);
            if (userFundsEntity == null)
            {
                userFundsEntity = new()
                {
                    ApplicationUserId = currentUserId,
                    AvailableFunds = fundsToAdd
                };
                await dbContext.UserFunds.AddAsync(userFundsEntity, cancellationToken);
            }
            else
            {
                userFundsEntity.AvailableFunds += fundsToAdd;
            }
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
