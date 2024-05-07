using FairPlayCombined.Services.Common;
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
            var sandboxBuyer = configuration["PayPal:SandboxBuyer"]!;
            SandboxEnvironment sandboxEnvironment = new(clientId, clientSecret);
            PayPalHttpClient payPalHttpClient = new(sandboxEnvironment);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<PayPalOrderService>();
            PayPalOrderService payPalOrderService = new(payPalHttpClient, logger);
            var result = await payPalOrderService.CreateOrderAsync(
                "1234", 1.2M, "Automated Tests",
                returnUrl: "https://example.com/returnUrl",
                cancelUrl: "https://example.com/returnUrl");
            Assert.IsNotNull(result);
        }
    }
}
