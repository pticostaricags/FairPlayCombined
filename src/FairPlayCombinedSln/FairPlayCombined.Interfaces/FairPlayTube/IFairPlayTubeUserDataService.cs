using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IFairPlayTubeUserDataService
    {
        Task EnqueueMyDataExportAsync(CancellationToken cancellationToken);
        Task<byte[]> GetMyVideoDataAsync(long videoInfoId, CancellationToken cancellationToken);
        Task<byte[]> GetUserDataAsync(string userId, CancellationToken cancellationToken);
    }
}
