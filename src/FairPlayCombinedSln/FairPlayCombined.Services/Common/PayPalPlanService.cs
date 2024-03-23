using Microsoft.Extensions.Logging;
using PayPal.Core;
using PayPal.v1.BillingPlans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public class PayPalPlanService(PayPalHttpClient httpClient, ILogger<PayPalPlanService> logger)
    {
        public async Task<PlanList> ListPlansAsync()
        {

            PlanListRequest request = new();
            var response = await httpClient!.Execute(request);
            var result = response.Result<PlanList>();
            return result;
        }
        public async Task<Plan> CreatePlan()
        {
            //Check https://github.com/paypal/PayPal-NET-SDK/blob/releasinator/Samples/Source/BillingPlanCreate.aspx.cs
            PayPal.v1.BillingPlans.Plan plan = new()
            {
                MerchantPreferences = new MerchantPreferences()
                {
                    AutoBillAmount = "YES",
                    InitialFailAmountAction="CONTINUE",
                    MaxFailAttempts = "0",
                    CancelUrl = "http://localhost/Cancel",
                    ReturnUrl = "http://localhost/Return"
                },
                Description = "FairPlayTube basic plan",
                Name = "FairPlayTube Basic 900",
                Type = "fixed",
                PaymentDefinitions =
                [
                    new PaymentDefinition()
                    {
                        Name = "Basic Plan Test 111",
                        Type = "REGULAR",
                        Cycles = "1",
                        Frequency = "MONTH",
                        FrequencyInterval = "1",
                        Amount = new PayPal.v1.BillingPlans.Currency()
                        {
                            CurrencyCode="USD",
                            Value = "5"
                        }
                    }
                ]
            };
            PlanCreateRequest planCreateRequest = new();
            planCreateRequest.RequestBody(plan);
            var response = await httpClient!.Execute(planCreateRequest);
            var planResult= response.Result<Plan>();
            logger.LogInformation(message:"Plan created: {planInfo}", planResult!.ToString());
            return planResult;
        }
    }
}
