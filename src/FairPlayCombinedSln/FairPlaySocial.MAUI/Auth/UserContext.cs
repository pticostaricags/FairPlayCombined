using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlaySocial.MAUI.Auth
{
    public static class UserContext
    {
        public static string? AccessToken { get; set; }
        public static long? AccessTokenExpiresIn { get; set; }
        public static string? RefreshToken { get; set; }
        public static DateTimeOffset? TokenExpiraton { get; set; }
        public static bool IsExpired => DateTimeOffset.UtcNow >= TokenExpiraton;
        public static bool IsAuthenticated => !IsExpired && !String.IsNullOrWhiteSpace(AccessToken);
    }
}
