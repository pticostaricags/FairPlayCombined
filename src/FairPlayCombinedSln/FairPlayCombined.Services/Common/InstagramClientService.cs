using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.InstagramApi;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class InstagramClientService(HttpClient httpClient,
        ILogger<InstagramClientService> logger) : IInstagramClientService
    {
        private const string VERSION = "v22.0";
        public async Task<UserInfoModel?> GetUserInfoAsync(string username, string accessToken,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Start of method: {MethodName}", nameof(GetUserInfoAsync));
            string requestUrl = $"https://graph.instagram.com/{VERSION}/{username}" +
                $"?fields=id,user_id,username,name,account_type,profile_picture_url,followers_count,follows_count,media_count" +
                $"&access_token={accessToken}";
            var result = await httpClient.GetFromJsonAsync<UserInfoModel>(requestUrl, cancellationToken);
            logger.LogInformation("End of method: {MethodName}", nameof(GetUserInfoAsync));
            return result;
        }

        public async Task<UserMediaModel?> GetUserMediaAsync(string username, string accessToken,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Start of method: {MethodName}", nameof(GetUserMediaAsync));
            string requestUrl = $"https://graph.instagram.com/{VERSION}/{username}/media" +
                $"?access_token={accessToken}";
            var result = await httpClient.GetFromJsonAsync<UserMediaModel>(requestUrl, cancellationToken);
            logger.LogInformation("End of method: {MethodName}", nameof(GetUserMediaAsync));
            return result;
        }

        private async Task<CreateMediaContainerResponseModel?> CreateMediaContainerAsync(CreateMediaContainerModel createMediaContainerModel,
            string username, string accessToken,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Start of method: {MethodName}", nameof(CreateMediaContainerAsync));
            string requestUrl = $"https://graph.instagram.com/{VERSION}/{username}/media?access_token={accessToken}";
            var response = await httpClient.PostAsJsonAsync(requestUrl, createMediaContainerModel, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateMediaContainerResponseModel>(cancellationToken);
            logger.LogInformation("End of method: {MethodName}", nameof(CreateMediaContainerAsync));
            return result;
        }

        private async Task<PublishMediaContainerResponseModel?> PublishMediaContainerAsync(PublishMediaContainerModel publishMediaContainerModel,
            string username, string accessToken,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Start of method: {MethodName}", nameof(PublishMediaContainerAsync));
            string requestUrl = $"https://graph.instagram.com/{VERSION}/{username}/media_publish?access_token={accessToken}";
            var response = await httpClient.PostAsJsonAsync(requestUrl, publishMediaContainerModel, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<PublishMediaContainerResponseModel>(cancellationToken);
            logger.LogInformation("End of method: {MethodName}", nameof(PublishMediaContainerAsync));
            return result;
        }

        public async Task<PublishMediaContainerResponseModel?> 
            CreateSingleMediaPostAsync(string username, string accessToken, 
            string imageUrl, CancellationToken cancellationToken)
        {
            CreateMediaContainerModel createMediaContainerModel = new()
            {
                image_url = imageUrl
            };
            var createdMediaModel = 
            await this.CreateMediaContainerAsync(createMediaContainerModel, 
                username:username, accessToken:accessToken, cancellationToken: cancellationToken);
            PublishMediaContainerModel publishMediaContainerModel = new()
            {
                creation_id = createdMediaModel!.id
            };
            var result = await this.PublishMediaContainerAsync(publishMediaContainerModel, username, accessToken, cancellationToken);
            return result;
        }

        public async Task<PublishMediaContainerResponseModel?>
            CreateReelPostAsync(string username, string accessToken,
            string videoUrl, CancellationToken cancellationToken)
        {
            CreateMediaContainerModel createMediaContainerModel = new()
            {
                video_url = videoUrl,
                media_type = "REELS"
            };
            var createdMediaModel =
            await this.CreateMediaContainerAsync(createMediaContainerModel,
                username: username, accessToken: accessToken, cancellationToken: cancellationToken);
            PublishMediaContainerModel publishMediaContainerModel = new()
            {
                creation_id = createdMediaModel!.id
            };
            var result = await this.PublishMediaContainerAsync(publishMediaContainerModel, username, accessToken, cancellationToken);
            return result;
        }
    }
}
