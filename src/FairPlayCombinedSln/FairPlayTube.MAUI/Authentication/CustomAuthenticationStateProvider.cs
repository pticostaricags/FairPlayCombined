using FairPlayTube.MAUI.Features.LogOn;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayTube.MAUI.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new();
            var user = new ClaimsPrincipal(identity);
            var result = new AuthenticationState(user);
            return Task.FromResult(result);
        }
    }
}
