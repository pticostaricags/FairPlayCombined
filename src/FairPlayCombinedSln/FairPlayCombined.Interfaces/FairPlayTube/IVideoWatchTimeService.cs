using FairPlayCombined.Models.FairPlayTube.VideoWatchTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoWatchTimeService
    {
        Task CreateVideoWatchTimeAsync(string videoId, VideoWatchTimeModel videoWatchTimeModel,
            CancellationToken cancellationToken);
        Task UpdateVideoWatchTimeAsync(VideoWatchTimeModel videoWatchTimeModel,
            CancellationToken cancellationToken);
    }
}
