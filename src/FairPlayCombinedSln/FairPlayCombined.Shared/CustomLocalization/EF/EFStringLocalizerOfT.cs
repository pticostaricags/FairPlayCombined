using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace FairPlayCombined.Shared.CustomLocalization.EF
{
    /// <summary>
    /// Handles EF-based localization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks>
    /// Initializes <see cref="EFStringLocalizer{T}"/>
    /// </remarks>
    /// <param name="dbContextFactory"></param>
    public class EFStringLocalizer<T>(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        ICustomCache customCache, ILogger<EFStringLocalizer> logger) : IStringLocalizer<T>
    {

        /// <summary>
        /// Retrieves the value for the given key
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public LocalizedString this[string name]
        {
            get
            {
                var value = GetString(name);
                return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
            }
        }

        /// <summary>
        /// Tetrieves the value for the given key using the specified arguments
        /// </summary>
        /// <param name="name"></param>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                var format = GetString(name);
                var value = string.Format(format ?? name, arguments);
                return new LocalizedString(name, value, resourceNotFound: format == null);
            }
        }

        /// <summary>
        /// Sets the Culture to use
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            CultureInfo.DefaultThreadCurrentCulture = culture;
            return new EFStringLocalizer(dbContextFactory, customCache, logger);
        }

        /// <summary>
        /// Gets all of the values
        /// </summary>
        /// <param name="includeParentCultures"></param>
        /// <returns></returns>
        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            var db = dbContextFactory.CreateDbContext();
            var typeFullName = typeof(T).FullName;
            var cacheKey = $"{typeFullName}-{nameof(GetAllStrings)}-{CultureInfo.CurrentCulture.Name}";
            var result = customCache!.GetOrCreateAsync<IQueryable<LocalizedString>>(
                cacheKey, retrieveDataTask: () =>
                {
                    logger.LogInformation("Executing method {MethodName}", nameof(GetAllStrings));
                    var data = db.Resource
                    .AsNoTracking()
                    .Include(r => r.Culture)
                    .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name
                    && r.Type == typeFullName)
                    .Select(r => new LocalizedString(r.Key, r.Value, true));
                    return Task.FromResult(data);
                }, expiration: Constants.CacheConfiguration.LocalizationCacheDuration,
                cancellationToken: CancellationToken.None)
                .Result;
            return result!;
        }

        private string? GetString(string name)
        {
            var db = dbContextFactory.CreateDbContext();
            var typeFullName = typeof(T).FullName;
            var cacheKey = $"{typeFullName}-{nameof(GetString)}-{name}-{CultureInfo.CurrentCulture.Name}";
            var result = customCache!.GetOrCreateAsync<string?>(cacheKey,
                retrieveDataTask: () =>
            {
                logger.LogInformation("Executing method {MethodName} for resource {ResourceName}", nameof(GetString), name);
                var data =
                db.Resource
                .AsNoTracking()
                .Include(r => r.Culture)
                .Where(r => r.Culture.Name == CultureInfo.CurrentCulture.Name &&
                r.Type == typeFullName
                )
                .FirstOrDefault(r => r.Key == name)?.Value;
                return Task.FromResult(data);
            }, expiration: Constants.CacheConfiguration.LocalizationCacheDuration,
            cancellationToken: CancellationToken.None).Result;
            return result;
        }
    }
}
