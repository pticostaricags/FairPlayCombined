﻿using FairPlayCombined.Interfaces;
using Microsoft.Extensions.Logging;
using PayoutsSdk.Core;
using PayoutsSdk.Payouts;

namespace FairPlayCombined.Services.Common
{
    public class PayPalService(PayPalHttpClient httpClient, ILogger<PayPalService> logger) : IPayPalService
    {
        public async Task<CreatePayoutResponse?> CreatePayoutAsync(
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
                logger.LogError(exception: ex, message: "Exception occurred in {MethodName}. " +
                    "Message: {Message}", nameof(CreatePayoutAsync), ex.Message);
                return null;
            }
        }
    }
}
