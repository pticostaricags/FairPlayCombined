using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.Common
{
    public interface ICustomCache
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> retrieveDataTask, 
            TimeSpan expiration,
            CancellationToken cancellationToken);
    }
}
