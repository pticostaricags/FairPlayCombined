using FairPlayCombined.Models.FairPlaySocial.VideoCaptions;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoCaptionsService
    {
        Task<string?> GetVideoCaptionsByVideoInfoIdAndLanguageAsync(
            long videoInfoId, string language, CancellationToken cancellationToken);
        Task<VideoCaptionsModel[]> GetVideoCaptionsByVideoInfoIdAsync(
            long videoInfoId, CancellationToken cancellationToken);
    }
}
