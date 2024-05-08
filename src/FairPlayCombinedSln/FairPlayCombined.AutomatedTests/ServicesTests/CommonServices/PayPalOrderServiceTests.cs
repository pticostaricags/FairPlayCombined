﻿using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayPal.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class PayPalOrderServiceTests: ServicesBase
    {
        [TestMethod]
        public async Task Test_CreateOrderAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var clientId = configuration["PayPal:ClientId"]!;
            var clientSecret = configuration["PayPal:ClientSecret"]!;
            SandboxEnvironment sandboxEnvironment = new(clientId, clientSecret);
            PayPalHttpClient payPalHttpClient = new(sandboxEnvironment);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<PayPalOrderService>();
            PayPalOrderService payPalOrderService = new(payPalHttpClient, logger,
                new HttpClient(),
                new Models.Common.PayPal.PayPalConfiguration()
                {
                    ClientId = clientId,
                    Secret = clientSecret
                });
            var result = await payPalOrderService.CreateOrderAsync(
                "1234", 1.2M, "Automated Tests",
                returnUrl: "https://example.com/returnUrl",
                cancelUrl: "https://example.com/returnUrl", 
                PayPalOrderService.CreateOrderIntent.Capture);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GetOrderDetailsAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var clientId = configuration["PayPal:ClientId"]!;
            var clientSecret = configuration["PayPal:ClientSecret"]!;
            SandboxEnvironment sandboxEnvironment = new(clientId, clientSecret);
            PayPalHttpClient payPalHttpClient = new(sandboxEnvironment);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<PayPalOrderService>();
            PayPalOrderService payPalOrderService = new(payPalHttpClient, logger,
                new HttpClient(),
                new Models.Common.PayPal.PayPalConfiguration()
                {
                    ClientId= clientId,
                    Secret= clientSecret
                });
            var createdOrder = await payPalOrderService.CreateOrderAsync(
                "1234", 1.2M, "Automated Tests",
                returnUrl: "https://example.com/returnUrl",
                cancelUrl: "https://example.com/returnUrl",
                PayPalOrderService.CreateOrderIntent.Authorize);
            Assert.IsNotNull(createdOrder);
            var accessToken = await payPalOrderService.GetAccessTokenAsync(CancellationToken.None);
            var result = await payPalOrderService
                .GetOrderDetailsAsync(createdOrder.Id, accessToken.access_token!, CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
