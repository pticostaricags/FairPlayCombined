using Microsoft.Extensions.Logging;
using PayPal.Core;
using PayPal.v1.Orders;
using PayPal.v1.Payments;

namespace FairPlayCombined.Services.Common
{
    public class PayPalOrderService(PayPalHttpClient payPalHttpClient,
        ILogger<PayPalOrderService> logger)
    {
        public enum CreateOrderIntent
        {
            Capture,
            Authorize
        }
        public async Task<PayPal.v1.Orders.Order?> CreateOrderAsync(string referenceId,
            decimal amount, string brandName,
            string returnUrl, string cancelUrl, CreateOrderIntent createOrderIntent)
        {
            try
            {
                OrdersCreateRequest ordersCreateRequest = new();
                PayPal.v1.Orders.Order order = new()
                {
                    Intent = createOrderIntent.ToString().ToUpper(),
                    PurchaseUnits =
                    [
                        new PurchaseUnit()
                    {
                        ReferenceId = referenceId,
                        Amount=new()
                        {
                            Currency="USD",
                            Total = amount.ToString()
                        }
                    }
                    ],
                    ApplicationContext = new()
                    {
                        BrandName = brandName,
                        Locale = "en-US",
                    },
                    RedirectUrls = new()
                    {
                        ReturnUrl = returnUrl,
                        CancelUrl = cancelUrl
                    }
                };
                ordersCreateRequest.RequestBody(order);
                var response = await payPalHttpClient.Execute(ordersCreateRequest);
                var result = response.Result<PayPal.v1.Orders.Order>();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, message: "Exception occurred in {MethodName}. " +
                    "Message: {Message}", nameof(CreateOrderAsync), ex.Message);
                return null;
            }
        }

        public async Task<PayPal.v1.Payments.Order?> GetOrderDetailsAsync(string orderId, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                OrderGetRequest orderGetRequest = new(orderId);
                var response = await payPalHttpClient.Execute(orderGetRequest);
                var result = response.Result<PayPal.v1.Payments.Order>();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, message: "Error: {ErrorMessage}", ex.Message);
                return null;
            }
        }
    }
}
