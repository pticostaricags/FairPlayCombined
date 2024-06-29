using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace FairPlayTube.ClientServices.CustomLocalization
{
    public class ApiLocalizerFactory(
        [FromKeyedServices("AnonymousApiClient")]
        KiotaClient.ApiClient anonymousClient,
        IMemoryCache memoryCache
        ) : IStringLocalizerFactory
    {
        public IStringLocalizer Create(Type resourceSource)
        {
            var localizerType = typeof(ApiLocalizer<>)
                     .MakeGenericType(resourceSource);
            var instance = Activator.CreateInstance(localizerType,
            [
                anonymousClient, memoryCache
            ]) as IStringLocalizer;
            return instance!;
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            return new ApiLocalizer(anonymousClient, memoryCache);
        }
    }

}
