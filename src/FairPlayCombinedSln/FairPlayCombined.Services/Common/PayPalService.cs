﻿using FairPlayCombined.Interfaces;
using Google.Apis.Logging;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using PayoutsSdk.Core;
using PayoutsSdk.Payouts;
using PayPal.v1.BillingPlans;
using PayPal.v1.Payments;

namespace FairPlayCombined.Services.Common
{
    public class PayPalService(PayPalHttpClient httpClient, ILogger<PayPalService> logger) : IPayPalService
    {
        public async Task<CreatePayoutResponse> CreatePayoutAsync(
            string emailMessage,
            string emailSubject,
            string receiverEmailAddress,
            decimal amount)
        {
            try
            {
                var body = new CreatePayoutRequest()
                {
                    SenderBatchHeader = new SenderBatchHeader()
                    {
                        EmailMessage = emailMessage,
                        EmailSubject = emailSubject
                    },
                    Items = [
                    new()
                    {
                        RecipientType="EMAIL",
                        Amount=new PayoutsSdk.Payouts.Currency(){
                            CurrencyCode="USD",
                            Value=amount.ToString(),
                        },
                        Receiver=receiverEmailAddress,
                    }
                ]
                };
                PayoutsPostRequest request = new();
                request.RequestBody(body);
                var response = await httpClient!.Execute(request);
                var result = response.Result<CreatePayoutResponse>();
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, message: $"Exception occurred in {nameof(CreatePayoutAsync)}");
                throw;
            }
        }
    }
}