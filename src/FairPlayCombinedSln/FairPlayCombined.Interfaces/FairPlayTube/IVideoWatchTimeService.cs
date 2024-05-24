using FairPlayCombined.Models.FairPlayTube.VideoWatchTime;

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
