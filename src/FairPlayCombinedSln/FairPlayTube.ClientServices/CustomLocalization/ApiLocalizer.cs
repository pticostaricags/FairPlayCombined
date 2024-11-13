using FairPlayCombined.Common;
using FairPlayTube.ClientServices.KiotaClient.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Diagnostics;
using System.Globalization;

namespace FairPlayTube.ClientServices.CustomLocalization
{
    public class ApiLocalizer(
        [FromKeyedServices("AnonymousClient")]
        KiotaClient.ApiClient anonymousClient,
        IMemoryCache memoryCache) : IStringLocalizer
    {
        private readonly Lock _lock = new();
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
            string cacheKey = $"{nameof(GetAllStrings)} -{CultureInfo.CurrentCulture.Name}";
            var response = memoryCache.GetOrCreateAsync(
                cacheKey, (entry) =>
                {
                    entry.SlidingExpiration = Constants.CacheConfiguration.LocalizationCacheDuration;
                    var response = anonymousClient.Localization.GetAllResources.GetAsync().Result;
                    return Task.FromResult(response);
                }).Result;
            var result = response!.Select(p =>
            new LocalizedString(p.Key!, p.Value!))
                .ToArray();
            return result;
        }

        private string GetString(string name)
        {
            try
            {
                using (this._lock.EnterScope())
                {
                    var cacheKey = $"{name}-{CultureInfo.CurrentCulture.Name}";
                    var result = memoryCache.GetOrCreate(cacheKey, cacheEntry =>
                    {
                        cacheEntry.SlidingExpiration = Constants.CacheConfiguration.LocalizationCacheDuration;
                        var data = this.GetAllStrings().SingleOrDefault(p => p.Name == name)?.Name;
                        return data ?? name;
                    });
                    return result!;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return name;
            }
        }
    }
}
