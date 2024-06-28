﻿namespace FairPlayCombined.Interfaces.Common
{
    public interface IUserFundService
    {
        Task InitializeUserFundsAsync(string userId, CancellationToken cancellationToken);
        Task<decimal> GetMyAvailableFundsAsync(CancellationToken cancellationToken);
        Task AddMyFundsAsync(string orderId, CancellationToken cancellationToken);
    }
}
