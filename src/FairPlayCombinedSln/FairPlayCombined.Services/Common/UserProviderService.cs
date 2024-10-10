using FairPlayCombined.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace FairPlayCombined.Services.Common
{
    public class UserProviderService(IHttpContextAccessor httpContextAccessor) : IUserProviderService
    {
        public string? GetAccessToken()
        {
            throw new NotImplementedException();
        }

        public string? GetCurrentUserId()
        {
            string? result = default;
            if (httpContextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated == true)
            {
                var claimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
                result = httpContextAccessor!.HttpContext!.User.Claims.Single(p => p.Type == claimType)!.Value!;
            }
            return result;
        }

        public bool IsAuthenticatedWithGoogle()
        {
            var result = httpContextAccessor.HttpContext!.User
                .HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod",
                "Google");
            return result;
        }

        public bool IsAuthenticatedWithLinkedIn()
        {
            var result = httpContextAccessor.HttpContext!.User
                .HasClaim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod",
                "LinkedIn");
            return result;
        }
    }
}
