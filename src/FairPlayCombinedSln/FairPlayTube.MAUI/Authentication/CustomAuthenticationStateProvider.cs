using FairPlayTube.ClientServices.KiotaClient;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace FairPlayTube.MAUI.Authentication
{
    public class CustomAuthenticationStateProvider(
        [FromKeyedServices("AuthenticatedApiClient")]
        ApiClient authenticatedClient

        ) : AuthenticationStateProvider
    {
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new();
            if (UserContext.IsAuthenticated) {
                var response = await authenticatedClient.Identity.GetMyRoles.GetAsync();
                foreach (var singleUserRole in response!)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, singleUserRole));
                }
            }
            var user = new ClaimsPrincipal(identity);
            var result = new AuthenticationState(user);
            return result;
        }
    }
}
