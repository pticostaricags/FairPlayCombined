using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.Common.PayPal;
using FairPlayCombined.Services.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Services.Extensions
{
    public static class PayPalCoreExtensions
    {
        public static void AddPayPalCore(this WebApplicationBuilder builder)
        {
            var paypalClientId = builder.Configuration["PayPal:ClientId"] ??
                throw new InvalidOperationException("'PayPal:ClientId' not found");

            var paypalClientSecret = builder.Configuration["PayPal:ClientSecret"] ??
                throw new InvalidOperationException("'PayPal:ClientSecret' not found");
            builder.Services.AddSingleton<PayPalConfiguration>(new PayPalConfiguration()
            {
                ClientId = paypalClientId,
                Secret = paypalClientSecret
            });
            builder.Services.AddTransient<PayPal.Core.PayPalHttpClient>(sp =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    PayPal.Core.SandboxEnvironment sandboxEnvironment = new(paypalClientId, paypalClientSecret);
                    PayPal.Core.PayPalHttpClient payPalHttpClient = new(sandboxEnvironment);
                    return payPalHttpClient;
                }
                else
                {
                    PayPal.Core.PayPalEnvironment payPalEnvironment = new(paypalClientId, paypalClientSecret,
                        "/", "/");
                    PayPal.Core.PayPalHttpClient payPalHttpClient = new(payPalEnvironment);
                    return payPalHttpClient;
                }
            });
            builder.Services.AddTransient<IPayPalOrderService, PayPalOrderService>(sp =>
            {
                var payPalHttpClient = sp.GetRequiredService<PayPal.Core.PayPalHttpClient>();
                var logger = sp.GetRequiredService<ILogger<PayPalOrderService>>();
                var basicHttpClient = sp.GetRequiredService<HttpClient>();
                var payPalConfiguration = sp.GetRequiredService<PayPalConfiguration>();
                PayPalOrderService payPalOrderService = new(payPalHttpClient, logger,
                    basicHttpClient, payPalConfiguration
                    );
                return payPalOrderService;
            });
        }
    }
}
