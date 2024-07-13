using FairPlayCombined.Interfaces.FairPlayTube;
using Microsoft.AspNetCore.Components;

namespace FairPlayTube.Helpers
{
    public class ApiResolver(NavigationManager navigationManager) : IApiResolver
    {
        public string GetBaseUrl()
        {
            return navigationManager.BaseUri.TrimEnd('/');
        }
    }
}
