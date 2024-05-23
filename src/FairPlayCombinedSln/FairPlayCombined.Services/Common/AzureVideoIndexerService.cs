using Azure.Core;
using Azure.Identity;
using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.AzureVideoIndexer;
using Microsoft.Identity.Client;
using NetTopologySuite.Geometries;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;

namespace FairPlayCombined.Services.Common
{
    public partial class AzureVideoIndexerService(
        AzureVideoIndexerServiceConfiguration azureVideoIndexerServiceConfiguration,
        HttpClient httpClient) : IAzureVideoIndexerService
    {
        private const string BEARER_SCHEME = "Bearer";
        public async Task<string> AuthenticateToAzureArmAsync()
        {
            var tokenRequestContext = new TokenRequestContext(["https://management.azure.com/.default"]);
            var tokenRequestResult = await new DefaultAzureCredential().GetTokenAsync(tokenRequestContext, CancellationToken.None);
            return tokenRequestResult.Token;
        }
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
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER_SCHEME, bearerToken);
            var response = await httpClient.PostAsJsonAsync(requestUrl, getAccessTokenRequestModel, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content
                    .ReadFromJsonAsync<GetAccessTokenResponseModel>(cancellationToken: cancellationToken);
                return result;
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                throw new AzureVideoIndexerException($"Error. Reason: {response.ReasonPhrase}. Details: {errorDetails}");
            }
        }

        public async Task<UploadVideoResponseModel?> IndexVideoFromBytesAsync(
            IndexVideoFromBytesFormatModel indexVideoFromBase64FormatModel,
            string viAccountAccessToken, CancellationToken cancellationToken)
        {
            string requestUrl = $"https://api.videoindexer.ai" +
                $"/{azureVideoIndexerServiceConfiguration.Location}" +
                $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                $"/Videos" +
                $"?name={indexVideoFromBase64FormatModel.Name}" +
                $"&filename={indexVideoFromBase64FormatModel.Name}";
            MultipartFormDataContent multipartContent =
                   new()
                   {
                       {
                           new StreamContent(new MemoryStream(indexVideoFromBase64FormatModel!.FileBytes!)),
                           "file",
                           indexVideoFromBase64FormatModel.Name!
                       }
                   };
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccountAccessToken);
            var response = await httpClient.PostAsync(requestUrl, multipartContent, cancellationToken: cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UploadVideoResponseModel>(cancellationToken: cancellationToken);
                return result;
            }
            else
            {
                var reasonPhrase = response.ReasonPhrase;
                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                throw new AzureVideoIndexerException($"Error: {reasonPhrase} - Details:{responseContent}");
            }
        }

        public async Task<bool> DeleteVideoByIdAsync(string videoId, string viAccessToken,
            CancellationToken cancellationToken)
        {
            try
            {
                string requestUrl =
                    $"https://api.videoindexer.ai/{azureVideoIndexerServiceConfiguration.Location}" +
                    $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                    $"/Videos/{videoId}";
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                var response = await httpClient.DeleteAsync(requestUrl, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var reasonPhrase = response.ReasonPhrase;
                    var responseContent = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                    throw new AzureVideoIndexerException($"Error: {reasonPhrase} - Details:{responseContent}");
                }
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }
        public async Task<UploadVideoResponseModel?> IndexVideoFromUriAsync(
            IndexVideoFromUriParameters indexVideoFromUriParameters,
            CancellationToken cancellationToken = default)
        {
            try
            {
                string requestUrl =
                    $"https://api.videoindexer.ai/{azureVideoIndexerServiceConfiguration.Location}" +
                    $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                    $"/Videos" +
                    $"?name={indexVideoFromUriParameters.Name}" +
                    $"&description={HttpUtility.UrlEncode(indexVideoFromUriParameters.Description)}";
                requestUrl +=
                $"&language={indexVideoFromUriParameters.Language}" +
                $"&videoUrl={HttpUtility.UrlEncode(indexVideoFromUriParameters.VideoUri!.ToString())}" +
                $"&fileName={HttpUtility.UrlEncode(indexVideoFromUriParameters.FileName)}" +
                $"&indexingPreset={indexVideoFromUriParameters.IndexingPreset}" +
                $"&Privacy=Public";
                requestUrl +=
                $"&sendSuccessEmail={true}";
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, indexVideoFromUriParameters.ArmAccessToken);
                var response = await httpClient.PostAsync(requestUrl, null, cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UploadVideoResponseModel>(cancellationToken: cancellationToken);
                    return result;
                }
                else
                {
                    var reasonPhrase = response.ReasonPhrase;
                    var responseContent = await response.Content.ReadAsStringAsync(cancellationToken: cancellationToken);
                    throw new AzureVideoIndexerException($"Error: {reasonPhrase} - Details:{responseContent}");
                }
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }

        public async Task<SearchVideosResponseModel?> SearchVideosByNameAsync(
            string viAccessToken,
            string name,
            CancellationToken cancellationToken = default)
        {
            try
            {
                string requestUrl = $"https://api.videoindexer.ai/" +
                    $"/{azureVideoIndexerServiceConfiguration.Location}" +
                    $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                    $"/Videos/Search" +
                    $"?query={name}" +
                    $"&textScope=Name";
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                var result = await httpClient.GetFromJsonAsync<SearchVideosResponseModel>(requestUrl, cancellationToken: cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }

        public async Task<SearchVideosResponseModel?> SearchVideosByIdsAsync(
            string viAccessToken,
            string[] videoIds,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var idKeyPairs = videoIds.Select(p => $"id={p}");
                string idQueryString = String.Join("&", idKeyPairs);
                string requestUrl = $"https://api.videoindexer.ai/" +
                    $"/{azureVideoIndexerServiceConfiguration.Location}" +
                    $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                    $"/Videos/Search" +
                    $"?{idQueryString}";
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                var result = await httpClient.GetFromJsonAsync<SearchVideosResponseModel>(requestUrl, cancellationToken: cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }

        public async Task<string> GetVideoVTTCaptionsAsync(string videoId,
            string viAccessToken, string language,
        CancellationToken cancellationToken = default)
        {
            string requestUrl = $"https://api.videoindexer.ai/{azureVideoIndexerServiceConfiguration.Location}" +
                $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                $"/Videos/{videoId}" +
                $"/Captions" +
                $"?format=Vtt" +
                $"&language={language}" +
                $"&includeAudioEffects=true" +
                $"&includeSpeakers=true";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                var result = await httpClient.GetStringAsync(requestUrl, cancellationToken: cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }

        public async Task<SupportedLanguageModel[]?> GetSupportedLanguagesAsync(
        string viAccessToken,
        CancellationToken cancellationToken = default)
        {
            string requestUrl = $"https://api.videoindexer.ai/{azureVideoIndexerServiceConfiguration.Location}" +
                $"/SupportedLanguages";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                var result = await httpClient.GetFromJsonAsync<SupportedLanguageModel[]>(requestUrl, cancellationToken: cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }

        public async Task<GetVideoIndexResponseModel?> GetVideoIndexAsync(string videoId,
        string viAccessToken,
        CancellationToken cancellationToken = default)
        {
            //Check https://api-portal.videoindexer.ai/api-details#api=Operations&operation=Get-Video-Index
            string requestUrl = $"https://api.videoindexer.ai/{azureVideoIndexerServiceConfiguration.Location}" +
                $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                $"/Videos/{videoId}" +
                $"/Index?" +
                $"includeStreamingUrls=true";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                var result = await httpClient.GetFromJsonAsync<GetVideoIndexResponseModel>(requestUrl, cancellationToken: cancellationToken);
                return result;
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }

        /// <summary>
        /// Gets the streaming url for the specified video
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="language"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<string> GetVideoStreamingUrlAsync(string videoId, string viAccessToken,
            CancellationToken cancellationToken = default)
        {
            try
            {

                string requestUrl = $"https://api.videoindexer.ai/{azureVideoIndexerServiceConfiguration.Location}" +
                    $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                    $"/Videos/{videoId}" +
                    $"/streaming-url";
                try
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                    var result = await httpClient.GetStringAsync(requestUrl, cancellationToken: cancellationToken);
                    return result;
                }
                catch (Exception ex)
                {
                    throw new AzureVideoIndexerException(ex.Message);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Gets the thumnail for the specified video
        /// </summary>
        /// <param name="videoId"></param>
        /// <param name="thumbnailId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<byte[]?> GetVideoThumbnailAsync(string videoId, string thumbnailId,
            string viAccessToken,
            CancellationToken cancellationToken = default)
        {
            string format = "Jpeg"; // Jpeg or Base64
            string requestUrl = $"https://api.videoindexer.ai/{azureVideoIndexerServiceConfiguration.Location}" +
                $"/Accounts/{azureVideoIndexerServiceConfiguration.AccountId}" +
                $"/Videos/{videoId}" +
                $"/Thumbnails/{thumbnailId}" +
                $"?format={format}";
            try
            {
                httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_SCHEME, viAccessToken);
                var imageBytes = await httpClient.GetByteArrayAsync(requestUrl, cancellationToken: cancellationToken);
                return imageBytes;
            }
            catch (Exception ex)
            {
                throw new AzureVideoIndexerException(ex.Message);
            }
        }
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