using FairPlayCombined.Models.LinkedInAuth;
using Microsoft.AspNetCore.Builder;

namespace FairPlayCombined.Services.Extensions
{
    public static class LinkedInAuthExtensions
    {
        public static LinkedInAuthClientSecretInfo GetLinkedInAuthClientSecretInfo(this WebApplicationBuilder builder)
        {
            var linkedInAuthClientId = builder.GetConfigurationValue("LinkedInAuthClientId");

            var linkedInAuthClientSecret = builder.GetConfigurationValue("LinkedInAuthClientSecret");

           
            LinkedInAuthClientSecretInfo linkedInAuthClientSecretInfo = new()
            {
                ClientId = linkedInAuthClientId,
                ClientSecret = linkedInAuthClientSecret,
            };
            return linkedInAuthClientSecretInfo;
        }
    }
}
