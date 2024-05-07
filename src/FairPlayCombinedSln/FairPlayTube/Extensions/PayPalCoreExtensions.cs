﻿using FairPlayCombined.Services.Common;

namespace FairPlayTube.Extensions
{
    public static class PayPalCoreExtensions
    {
        public static void AddPayPalCore(this WebApplicationBuilder builder)
        {
            var paypalClientId = builder.Configuration["PayPal:ClientId"] ??
                throw new InvalidOperationException("'PayPal:ClientId' not found");

            var paypalClientSecret = builder.Configuration["PayPal:ClientSecret"] ??
                throw new InvalidOperationException("'PayPal:ClientSecret' not found");

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
            builder.Services.AddTransient<PayPalOrderService>(sp =>
            {
                var payPalHttpClient = sp.GetRequiredService<PayPal.Core.PayPalHttpClient>();
                var logger = sp.GetRequiredService<ILogger<PayPalOrderService>>();
                PayPalOrderService payPalOrderService = new(payPalHttpClient, logger);
                return payPalOrderService;
            });
        }
    }
}