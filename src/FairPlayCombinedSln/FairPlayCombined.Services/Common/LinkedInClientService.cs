using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.LinkedInApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class LinkedInClientService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService,
        ILogger<LinkedInClientService> logger,
        HttpClient httpClient) : ILinkedInClientService
    {
        private async Task<AspNetUserTokens> GetAccessTokenAsync(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory, IUserProviderService userProviderService)
        {
            var userId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var accessTokenEntity = await
                dbContext.AspNetUserTokens
                .AsNoTracking()
                .SingleAsync(p => p.UserId == userId && p.LoginProvider == "LinkedIn" &&
                p.Name == "access_token");
            return accessTokenEntity;
        }
        public async Task<UserInfoModel> GetUserInfoAsync(CancellationToken cancellationToken)
        {
            AspNetUserTokens accessTokenEntity = await GetAccessTokenAsync(dbContextFactory, userProviderService);
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessTokenEntity.Value);
#pragma warning disable S1075 // URIs should not be hardcoded
            string requestUrl = "https://api.linkedin.com/v2/userinfo";
#pragma warning restore S1075 // URIs should not be hardcoded
            var result = await httpClient.GetFromJsonAsync<UserInfoModel>(requestUrl, cancellationToken);
            return result!;
        }

        public async Task CreateTextShareAsync(string text, CancellationToken cancellationToken)
        {
            AspNetUserTokens accessTokenEntity = await GetAccessTokenAsync(dbContextFactory, userProviderService);
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessTokenEntity.Value);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            var userInfo = await this.GetUserInfoAsync(cancellationToken);
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

        public async Task CreateArticleOrUrlShareAsync(string text, string? title,
            string? description, string url, CancellationToken cancellationToken)
        {
            AspNetUserTokens accessTokenEntity = await GetAccessTokenAsync(dbContextFactory, userProviderService);
            httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessTokenEntity.Value);
            httpClient.DefaultRequestHeaders.Add("X-Restli-Protocol-Version", "2.0.0");
            var userInfo = await this.GetUserInfoAsync(cancellationToken);
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


