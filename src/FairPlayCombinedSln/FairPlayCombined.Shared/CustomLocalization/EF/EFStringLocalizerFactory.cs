using FairPlayCombined.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace FairPlayCombined.Shared.CustomLocalization.EF
{
    /// <summary>
    /// Handles EF-based lcoalization
    /// </summary>
    /// <remarks>
    /// Initializes <see cref="EFStringLocalizerFactory"/>
    /// </remarks>
    /// <param name="dbContextFactory"></param>
    public class EFStringLocalizerFactory(IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IMemoryCache memoryCache) : IStringLocalizerFactory
    {

        /// <summary>
        /// Creates the localizer
        /// </summary>
        /// <param name="resourceSource"></param>
        /// <returns></returns>
        public IStringLocalizer Create(Type resourceSource)
        {
            var localizerType = typeof(EFStringLocalizer<>)
                .MakeGenericType(resourceSource);
            var instance = Activator.CreateInstance(localizerType, dbContextFactory,
                memoryCache) as IStringLocalizer;
            return instance!;
        }

        /// <summary>
        /// Create the localizeer using the location
        /// </summary>
        /// <param name="baseName"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public IStringLocalizer Create(string baseName, string location)
        {
            return new EFStringLocalizer(dbContextFactory, memoryCache);
        }
    }
}
