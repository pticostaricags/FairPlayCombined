using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IYouTubeClientService
    {
        Task<SearchListResponse> SearchMyVideosAsync(string searchTerm, CancellationToken cancellationToken);
        Task UploadVideoAsync(Video video, Stream fileStream, Action<IUploadProgress> progressChanged, 
            Action<Video> responseReceived);
        Task<YouTubeService> AuthorizeAsync();
        Task<UploadStatus> UploadCaptionsAsync(string youtubeVideoId, string language,
            YouTubeService youTubeService, MemoryStream captionsStream,
            Action<IUploadProgress> progressChanged,
            Action<Caption> responseReceived, CancellationToken cancellationToken);
    }
}
