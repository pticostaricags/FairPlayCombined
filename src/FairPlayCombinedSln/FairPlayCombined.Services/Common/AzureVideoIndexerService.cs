using Azure;
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
using static FairPlayCombined.Common.Constants;

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

        public async Task<UploadVideoResponseModel?> IndexVideoFromBytes(
            IndexVideoFromBase64FormatModel indexVideoFromBase64FormatModel,
            string viAccountAccessToken, CancellationToken cancellationToken)
        {
            string requestUrl = $"https://api.videoindexer.ai" +
                $"/{azureVideoIndexerServiceConfiguration.Location}" +
                $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                $"/Videos" +
                $"?name={indexVideoFromBase64FormatModel.Name}" +
                $"&filename={indexVideoFromBase64FormatModel.Name}";
            MultipartFormDataContent multipartContent =
                   new MultipartFormDataContent();
            multipartContent.Add(
                new StreamContent(new MemoryStream(indexVideoFromBase64FormatModel!.FileBytes!)),
                "file", indexVideoFromBase64FormatModel.Name!);
            httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", viAccountAccessToken);
            var response = await httpClient.PostAsync(requestUrl, multipartContent, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UploadVideoResponseModel>();
                return result;
            }
            else
            {
                var reasonPhrase = response.ReasonPhrase;
                var responseContent = await response.Content.ReadAsStringAsync();
                throw new AzureVideoIndexerException($"Error: {reasonPhrase} - Details:{responseContent}");
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

    public class IndexVideoFromBase64FormatModel
    {
        public byte[]? FileBytes { get; set; }
        public string? Name { get; set; }
    }

    public class UploadVideoResponseModel
    {
        public string? accountId { get; set; }
        public string? id { get; set; }
        public object? partition { get; set; }
        public object? externalId { get; set; }
        public object? metadata { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
        public DateTime lastIndexed { get; set; }
        public string? privacyMode { get; set; }
        public string? userName { get; set; }
        public bool isOwned { get; set; }
        public bool isBase { get; set; }
        public bool hasSourceVideoFile { get; set; }
        public string? state { get; set; }
        public string? moderationState { get; set; }
        public string? reviewState { get; set; }
        public object? processingProgress { get; set; }
        public int durationInSeconds { get; set; }
        public string? thumbnailVideoId { get; set; }
        public string? thumbnailId { get; set; }
        public object[]? searchMatches { get; set; }
        public string? indexingPreset { get; set; }
        public string? streamingPreset { get; set; }
        public string? sourceLanguage { get; set; }
        public string[]? sourceLanguages { get; set; }
        public string? personModelId { get; set; }
    }
}
