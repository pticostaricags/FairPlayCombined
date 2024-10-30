using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class TwitterClientService(
        IUserProviderService userProviderService,
        ILogger<TwitterClientService> logger,
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IHttpClientFactory httpClientFactory)
    {
        private async Task<AspNetUserTokens?> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Start of method: {MethodName}", nameof(GetAccessTokenAsync));
            var userId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var accessTokenEntity = await
                dbContext.AspNetUserTokens
                .AsNoTracking()
                .SingleAsync(p => p.UserId == userId && p.LoginProvider == "Twitter" &&
                p.Name == "access_token", cancellationToken);
            logger.LogInformation("End of method: {MethodName}", nameof(GetAccessTokenAsync));
            return accessTokenEntity;
        }

        public async Task<MyUserDataModel> GetMyUserDataAsync(
            string accessToken, CancellationToken cancellationToken)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                accessToken);
            var requestUrl = "https://api.twitter.com/2/users/me";
            var result = await httpClient.GetFromJsonAsync<MyUserDataModel>(requestUrl, cancellationToken);
            return result!;
        }
    }

    public class MyUserDataModel
    {
        public UserDataModel? data { get; set; }
    }

    public class UserDataModel
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? username { get; set; }
    }

}
