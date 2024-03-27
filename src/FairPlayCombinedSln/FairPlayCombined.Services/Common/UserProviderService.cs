using FairPlayCombined.Interfaces;
using Microsoft.AspNetCore.Http;

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
    }
}
