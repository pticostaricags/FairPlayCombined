using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.PayPal;
using Microsoft.Extensions.Logging;
using PayPal.Core;
using PayPal.v1.Orders;
using System.Net.Http.Json;
using System.Text;

namespace FairPlayCombined.Services.Common
{
    public class PayPalOrderService(PayPalHttpClient payPalHttpClient,
        ILogger<PayPalOrderService> logger, HttpClient basicHttpClient,
        PayPalConfiguration payPalConfiguration) : IPayPalOrderService
    {

        public async Task<GetAccessTokenResponse> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            string requestUrl = $"{payPalConfiguration.Endpoint}/v1/oauth2/token";
            var credentials = Encoding.ASCII.GetBytes($"{payPalConfiguration.ClientId}:{payPalConfiguration.Secret}");
            basicHttpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("basic", Convert.ToBase64String(credentials));
            List<KeyValuePair<string, string>> data =
                    [
                        new ("grant_type","client_credentials")
                    ];
            System.Net.Http.FormUrlEncodedContent formUrlEncodedContent = new(data);
            HttpRequestMessage httpRequestMessage = new(HttpMethod.Post, requestUrl)
            {
                Content = formUrlEncodedContent
            };
            var response = await basicHttpClient.SendAsync(httpRequestMessage, completionOption: HttpCompletionOption.ResponseContentRead, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GetAccessTokenResponse>(cancellationToken);
                return result!;
            }
            else
            {
                string reason = response.ReasonPhrase!;
                string detailedError = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new FairPlayCombined.Common.CustomExceptions.RuleException($"Reason: {reason}. Details: {detailedError}");
            }
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

        public async Task<GetOrderDetailsResponse?> GetOrderDetailsAsync(string orderId, string accessToken, CancellationToken cancellationToken = default)
        {
            try
            {
                string requestUrl = $"{payPalConfiguration.Endpoint}/v1/checkout/orders/{orderId}";
                basicHttpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var result = await basicHttpClient.GetFromJsonAsync<GetOrderDetailsResponse>(requestUrl, cancellationToken: cancellationToken);
                return result!;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error: {ErrorMessage}", ex.Message);
                return null;
            }
        }
    }
}
