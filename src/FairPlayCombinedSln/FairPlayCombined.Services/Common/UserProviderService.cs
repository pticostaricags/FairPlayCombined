using FairPlayCombined.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
