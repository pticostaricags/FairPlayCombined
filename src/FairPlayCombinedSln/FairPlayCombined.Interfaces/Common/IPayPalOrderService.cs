using FairPlayCombined.Models.Common.PayPal;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IPayPalOrderService
    {
        Task<GetAccessTokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken);
        Task<PayPal.v1.Orders.Order?> CreateOrderAsync(string referenceId,
            decimal amount, string brandName,
            string returnUrl, string cancelUrl, CreateOrderIntent createOrderIntent);
        Task<GetOrderDetailsResponse?> GetOrderDetailsAsync(string orderId, string accessToken,
            CancellationToken cancellationToken = default);
    }
    public enum CreateOrderIntent
    {
        Capture,
        Authorize
    }
}
