using Microsoft.Extensions.Logging;
using PayPal.Core;
using PayPal.v1.Orders;

namespace FairPlayCombined.Services.Common
{
    public class PayPalOrderService(PayPalHttpClient payPalHttpClient,
        ILogger<PayPalOrderService> logger)
    {
        public async Task<Order?> CreateOrderAsync(string referenceId,
            decimal amount, string brandName,
            string returnUrl, string cancelUrl)
        {
            try
            {
                OrdersCreateRequest ordersCreateRequest = new();
                Order order = new()
                {
                    Intent = "CAPTURE",
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
                    RedirectUrls=new RedirectUrls()
                    {
                        ReturnUrl= returnUrl,
                        CancelUrl= cancelUrl
                    }
                };
                ordersCreateRequest.RequestBody(order);
                var response = await payPalHttpClient.Execute(ordersCreateRequest);
                var result = response.Result<Order>();
                return result;
            }
            catch (Exception ex) 
            {
                logger.LogError(exception: ex, message: "Exception occurred in {MethodName}. " +
                    "Message: {Message}", nameof(CreateOrderAsync), ex.Message);
                return null;
            }
        }
    }
}
