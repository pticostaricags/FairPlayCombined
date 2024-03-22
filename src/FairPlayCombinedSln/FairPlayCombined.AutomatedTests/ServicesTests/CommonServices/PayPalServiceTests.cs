using FairPlayCombined.Services;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using PayoutsSdk.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class PayPalServiceTests : ServicesBase
    {
        [TestMethod]
        public async Task Test_CreatePayoutAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var clientId = configuration["PayPal:ClientId"]!;
            var clientSecret = configuration["PayPal:ClientSecret"]!;
            var sandboxBuyer = configuration["PayPal:SandboxBuyer"]!;
            SandboxEnvironment sandboxEnvironment=new(clientId, clientSecret);
            PayPalHttpClient payPalHttpClient=new(sandboxEnvironment);
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<PayPalService>();
            PayPalService payPalService = new(payPalHttpClient, logger);
            var result = await payPalService.CreatePayoutAsync("Test Email Message",
                "Test Email Subject", sandboxBuyer,
                1);
            Assert.IsNotNull(result);
        }
    }
}
