using FairPlayTube.ClientServices.KiotaClient;
using FairPlayTube.ClientServices.KiotaClient.Models;
using FairPlayTube.MAUI.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FairPlayTube.MAUI.Authentication
{
    public class CustomAuthenticationStateProvider(
        IServiceProvider serviceProvider,
        [FromKeyedServices("AnonymousApiClient")]
        ApiClient anonymousApiClient
        ) : AuthenticationStateProvider
    {
        private ClaimsPrincipal currentUser = new ClaimsPrincipal(new ClaimsIdentity());

        public Task LoginAsync(LoginRequest loginRequest)
        {

            var loginTask = LogInAsyncCore();
            NotifyAuthenticationStateChanged(loginTask);

            return loginTask;

            async Task<AuthenticationState> LogInAsyncCore()
            {
                var result = await anonymousApiClient!.Login.PostAsync(loginRequest);
                UserContext.AccessToken = result.AccessToken;
                UserContext.AccessTokenExpiresIn = result.ExpiresIn;
                UserContext.RefreshToken = result.RefreshToken;
                UserContext.TokenExpiraton = DateTimeOffset.UtcNow.AddMinutes(result!.ExpiresIn!.Value);
                using var scope = serviceProvider.CreateScope();
                var authenticatedClient = scope.ServiceProvider
                    .GetRequiredKeyedService<ApiClient>("AuthenticatedApiClient");

                ClaimsIdentity identity = new();
                if (UserContext.IsAuthenticated)
                {
                    var response = await authenticatedClient.Identity.GetMyRoles.GetAsync();
                    foreach (var singleUserRole in response!)
                    {
                        identity.AddClaim(new Claim(ClaimTypes.Role, singleUserRole));
                    }
                }
                this.currentUser = new ClaimsPrincipal(identity);
                var authenticationState = new AuthenticationState(this.currentUser);
                return authenticationState;
            }
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync() =>
        Task.FromResult(new AuthenticationState(currentUser));
    }
}
