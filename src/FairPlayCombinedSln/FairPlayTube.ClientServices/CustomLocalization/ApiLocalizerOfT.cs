using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace FairPlayTube.ClientServices.CustomLocalization
{
    public class ApiLocalizer<T>(
        [FromKeyedServices("AnonymousApiClient")]
        KiotaClient.ApiClient anonymousClient,
        IMemoryCache memoryCache
        ) : IStringLocalizer<T>
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
                $"{nameof(ApiLocalizer<T>)}.{GetAllStrings}", (entry) =>
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
            return name;
        }
    }
}
