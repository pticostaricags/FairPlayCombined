using FairPlayCombined.Common.CustomExceptions;
using Microsoft.Identity.Client.Extensions.Msal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public class AzureVideoIndexerService(
        AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration,
        HttpClient httpClient)
    {
        public async Task<GetAccessTokenResponseModel?> GetAccessTokenForArmAccountAsync(string bearerToken, CancellationToken cancellationToken)
        {
            string requestUrl = $"https://management.azure.com/subscriptions/" +
                $"{azureVideoIndexerServiceConfiguration.SubscriptionId}/resourceGroups/" +
                $"{azureVideoIndexerServiceConfiguration.ResourceGroup}/providers/Microsoft.VideoIndexer/accounts/" +
                $"{azureVideoIndexerServiceConfiguration.ResourceName}/generateAccessToken" +
                $"?api-version=2024-01-01";
            GetAccessTokenRequestModel getAccessTokenRequestModel = new()
            {
                PermissionType = "Contributor",
                Scope = "Account"
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            var response = await httpClient.PostAsJsonAsync(requestUrl, getAccessTokenRequestModel, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content
                    .ReadFromJsonAsync<GetAccessTokenResponseModel>(cancellationToken: cancellationToken);
                return result;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                throw new AzureVideoIndexerException($"Error. Reason: {response.ReasonPhrase}. Details: {errorDetails}");
            }
        }
    }


    public class GetAccessTokenRequestModel
    {
        public string? PermissionType { get; set; }
        public string? Scope { get; set; }
    }

    public class GetAccessTokenResponseModel
    {
        public string? AccessToken { get; set; }
    }


    public class AzureVideoIndexerServiceConfiguration
    {
        public string? AccountId { get; set; }
        public string? Location { get; set; }
        public bool IsArmAccount { get; set; }
        public string? ResourceGroup { get; set; }
        public string? SubscriptionId { get; set; }
        public string? ResourceName { get; set; }
        public string? ApiVersion { get; set; } = "2024-01-01";
    }
}
