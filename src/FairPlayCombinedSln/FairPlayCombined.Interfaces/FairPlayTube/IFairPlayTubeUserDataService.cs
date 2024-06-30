using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IFairPlayTubeUserDataService
    {
        Task<byte[]> GetMyUserDataAsync(CancellationToken cancellationToken);
    }
}
