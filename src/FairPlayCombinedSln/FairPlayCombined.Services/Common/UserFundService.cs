﻿using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public partial class UserFundService(PayPalOrderService payPalOrderService,
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService)
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
