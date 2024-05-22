using FairPlayCombined.Models.FairPlaySocial.VideoCaptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
