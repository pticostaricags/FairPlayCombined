using FairPlayCombined.Interfaces.Common;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public class CustomCache(HybridCache hybridCache,
        ILogger<CustomCache> logger) : ICustomCache
    {
        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> retrieveDataTask,
            TimeSpan expiration, CancellationToken cancellationToken)
        {
            var result = await hybridCache.GetOrCreateAsync<T>(key: key,
                factory: async (token) =>
                {
                    logger.LogInformation("Cache Factory. Executing method {MethodName} for resource {ResourceName}", nameof(GetOrCreateAsync), key);
                    token.ThrowIfCancellationRequested();
                    var data = await retrieveDataTask();
                    return data;
                }, options: new()
                {
                    Expiration = expiration,
                    LocalCacheExpiration = expiration,
                },
                cancellationToken: cancellationToken);
            return result;
        }
    }
}
