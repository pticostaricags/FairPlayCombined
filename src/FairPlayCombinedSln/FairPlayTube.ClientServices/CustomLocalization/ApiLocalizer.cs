using FairPlayCombined.Common;
using FairPlayTube.ClientServices.KiotaClient.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace FairPlayTube.ClientServices.CustomLocalization
{
    public class ApiLocalizer(
        [FromKeyedServices("AnonymousClient")]
        KiotaClient.ApiClient anonymousClient,
        IMemoryCache memoryCache) : IStringLocalizer
    {
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var response = memoryCache.GetOrCreate(
                $"{nameof(ApiLocalizer)}.{GetAllStrings}", (entry) =>
                {
                    entry.SlidingExpiration = Constants.CacheConfiguration.LocalizationCacheDuration;
                    var response = anonymousClient.Localization.GetAllResources.GetAsync().Result;
                    return response!;
                });
            var result = response!.Select(p =>
            new LocalizedString(p.Key!, p.Value!))
                .ToArray();
            return result;
        }

        private string GetString(string name)
        {
            var allStrings = GetAllStrings(false);
            return allStrings.SingleOrDefault(p => p.Name == name)?.Name ?? name;
        }
    }
}
