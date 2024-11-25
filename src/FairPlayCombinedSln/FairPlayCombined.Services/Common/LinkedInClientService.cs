using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.LinkedInApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Text.Json;

namespace FairPlayCombined.Services.Common
{
    public class LinkedInClientService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ILogger<LinkedInClientService> logger,
        HttpClient httpClient) : ILinkedInClientService
    {
        private const string BEARER_KEY = "Bearer";

        public async Task<string> GetAccessTokenForUserAsync(string userId, CancellationToken cancellationToken)
        {
            logger.LogInformation("Start of method: {MethodName}", nameof(GetAccessTokenForUserAsync));
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var accessTokenEntity = await
                dbContext.AspNetUserTokens
                .AsNoTracking()
                .SingleAsync(p => p.UserId == userId && p.LoginProvider == "LinkedIn" &&
                p.Name == "access_token");
            logger.LogInformation("End of method: {MethodName}", nameof(GetAccessTokenForUserAsync));
            return accessTokenEntity!.Value;
        }
        private async Task<AspNetUserTokens> GetAccessTokenAsync(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory, IUserProviderService userProviderService)
        {
            logger.LogInformation("Start of method: {MethodName}", nameof(GetAccessTokenAsync));
            var userId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var accessTokenEntity = await
                dbContext.AspNetUserTokens
                .AsNoTracking()
                .SingleAsync(p => p.UserId == userId && p.LoginProvider == "LinkedIn" &&
                p.Name == "access_token");
            logger.LogInformation("End of method: {MethodName}", nameof(GetAccessTokenAsync));
            return accessTokenEntity;
        }
        public async Task<UserInfoModel> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(BEARER_KEY, accessToken);
#pragma warning disable S1075 // URIs should not be hardcoded
            string requestUrl = "https://api.linkedin.com/v2/userinfo";
#pragma warning restore S1075 // URIs should not be hardcoded
            var result = await httpClient.GetFromJsonAsync<UserInfoModel>(requestUrl, cancellationToken);
            return result!;
        }

        private async Task<RegisterUploadResponseModel> RegisterUploadAsync(string accessToken,
            string authorId,
            CancellationToken cancellationToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(BEARER_KEY, accessToken);
#pragma warning disable S1075 // URIs should not be hardcoded
            var requestUrl = "https://api.linkedin.com/v2/assets?action=registerUpload";
#pragma warning restore S1075 // URIs should not be hardcoded
            string json = $$"""
                        {
                "registerUploadRequest": {
                    "recipes": [
                        "urn:li:digitalmediaRecipe:feedshare-image"
                    ],
                    "owner": "urn:li:person:{{authorId}}",
                    "serviceRelationships": [
                        {
                            "relationshipType": "OWNER",
                            "identifier": "urn:li:userGeneratedContent"
                        }
                    ]
                }
            }
            """;
            StringContent stringContent = new(json);
            var response = await httpClient.PostAsync(requestUrl, stringContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<RegisterUploadResponseModel>(cancellationToken);
            return result!;
        }

        private async Task UploadImageAsync(
            RegisterUploadResponseModel registerUploadResponseModel,
            Stream stream, string fileName,
            CancellationToken cancellationToken)
        {
            using var content = new MultipartFormDataContent();
            using var streamContent = new StreamContent(stream);
            streamContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            content.Add(streamContent, "file", fileName);

            var response = await httpClient
                .PostAsync(registerUploadResponseModel.value!.uploadMechanism!
                .comlinkedindigitalmediauploadingMediaUploadHttpRequest!.uploadUrl,
                content: content, cancellationToken: cancellationToken);

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> CreateImageShareAsync(string text, string accessToken,
            Stream imageStream, string filename,
            string mediaDescription, string mediaTitle,
            CancellationToken cancellationToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_KEY, accessToken);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            var userInfo = await this.GetUserInfoAsync(accessToken, cancellationToken);
            var authorId = userInfo.sub;

            var registeredUpload = await RegisterUploadAsync(accessToken, authorId!,
                cancellationToken);
            await UploadImageAsync(registeredUpload, imageStream, filename, cancellationToken);


            string CleanString(string input)
            {
                if (string.IsNullOrWhiteSpace(input))
                    return input;

                // Replace newlines with escaped versions
                input = input.Replace("\n", "\\n").Replace("\r", "\\r");

                // Remove any trailing or leading whitespace
                input = input.Trim();

                return input;
            }

            var escapedText = CleanString(text);
            var escapedMediaDescription = CleanString(mediaDescription);
            var escapedMediaTitle = CleanString(mediaTitle);
            var json = $$"""
{
    "author": "urn:li:person:{{authorId}}",
    "lifecycleState": "PUBLISHED",
    "specificContent": {
        "com.linkedin.ugc.ShareContent": {
            "shareCommentary": {
                "text": "{{escapedText}}"
            },
            "shareMediaCategory": "IMAGE",
            "media": [
                {
                    "status": "READY",
                    "description": {
                        "text": "{{escapedMediaDescription}}"
                    },
                    "media": "{{registeredUpload.value!.asset}}",
                    "title": {
                        "text": "{{escapedMediaTitle}}"
                    }
                }
            ]
        }
    },
    "visibility": {
        "com.linkedin.ugc.MemberNetworkVisibility": "PUBLIC"
    }
}
""";

#pragma warning disable S1075 // URIs should not be hardcoded
            string requestUrl = "https://api.linkedin.com/v2/ugcPosts";
#pragma warning restore S1075 // URIs should not be hardcoded
            StringContent stringContent = new(json);
            var response = await httpClient.PostAsync(requestUrl, stringContent, cancellationToken: cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                logger.LogError("Error: {ErrorMessage}", error);
            }
            return true;
        }
        public async Task CreateTextShareAsync(string text, string accessToken, CancellationToken cancellationToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue(BEARER_KEY, accessToken);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            var userInfo = await this.GetUserInfoAsync(accessToken, cancellationToken);
            var authorId = userInfo.sub;

            var json = $$"""
{
    "author": "urn:li:person:{{authorId}}",
    "lifecycleState": "PUBLISHED",
    "specificContent": {
        "com.linkedin.ugc.ShareContent": {
            "shareCommentary": {
                "text": "{{text}}"
            },
            "shareMediaCategory": "NONE"
        }
    },
    "visibility": {
        "com.linkedin.ugc.MemberNetworkVisibility": "PUBLIC"
    }
}
""";

#pragma warning disable S1075 // URIs should not be hardcoded
            string requestUrl = "https://api.linkedin.com/v2/ugcPosts";
#pragma warning restore S1075 // URIs should not be hardcoded
            StringContent stringContent = new(json);
            var response = await httpClient.PostAsync(requestUrl, stringContent, cancellationToken: cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                logger.LogError("Error: {ErrorMessage}", error);
            }
        }
        public async Task CreateArticleOrUrlShareAsync(string text, string accessToken, string? title,
            string? description, string url, CancellationToken cancellationToken)
        {
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(BEARER_KEY, accessToken);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            var userInfo = await this.GetUserInfoAsync(accessToken,cancellationToken);
            var authorId = userInfo.sub;

            var json = $$"""
{
    "author": "urn:li:person:{{authorId}}",
    "lifecycleState": "PUBLISHED",
    "specificContent": {
        "com.linkedin.ugc.ShareContent": {
            "shareCommentary": {
                "text": "{{text}}"
            },
            "shareMediaCategory": "ARTICLE",
            "media": [
                {
                    "status": "READY",
                    "description": {
                        "text": "{{description}}"
                    },
                    "originalUrl": "{{url}}",
                    "title": {
                        "text": "{{title}}"
                    }
                }
            ]
        }
    },
    "visibility": {
        "com.linkedin.ugc.MemberNetworkVisibility": "PUBLIC"
    }
}
""";

#pragma warning disable S1075 // URIs should not be hardcoded
            string requestUrl = "https://api.linkedin.com/v2/ugcPosts";
#pragma warning restore S1075 // URIs should not be hardcoded
            StringContent stringContent = new(json);
            var response = await httpClient.PostAsync(requestUrl, stringContent, cancellationToken: cancellationToken);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(cancellationToken);
                logger.LogError("Error: {ErrorMessage}", error);
            }
        }
    }
}


