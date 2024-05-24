namespace FairPlayCombined.Interfaces.Common
{
    public interface IUserFundService
    {
        Task<decimal> GetMyAvailableFundsAsync(CancellationToken cancellationToken);
        Task AddMyFundsAsync(string orderId, CancellationToken cancellationToken);
    }
}
