using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayPal.v1.BillingPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class PayPalPlanServiceTests
    {
        private static PayPal.Core.PayPalEnvironment GetPayPalCoreEnvironment(
            IConfigurationRoot configuration, bool useSandbox)
        {
            var clientId = configuration["PayPal:ClientId"]!;
            var clientSecret = configuration["PayPal:ClientSecret"]!;
            PayPal.Core.PayPalEnvironment? result;
            if (useSandbox)
                result = new PayPal.Core.SandboxEnvironment(clientId, clientSecret);
            else
                result = new PayPal.Core.LiveEnvironment(clientId, clientSecret);
            return result;
        }

        [TestMethod]
        public async Task Test_CreatePlan()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<PayPalPlanService>();
            PayPalPlanService payPalPlanService = new(
                new PayPal.Core.PayPalHttpClient(GetPayPalCoreEnvironment(configuration,true)),
                logger);
            var result = await payPalPlanService.CreatePlan();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_ListPlansAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<PayPalPlanService>();
            PayPal.Core.PayPalEnvironment payPalEnvironment =
                GetPayPalCoreEnvironment(configuration, true);
            PayPalPlanService payPalPlanService = new(
                new PayPal.Core.PayPalHttpClient(payPalEnvironment),
                logger);
            var result = await payPalPlanService.ListPlansAsync();
            Assert.IsNotNull(result);
        }
    }
}
