using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Http;
using System.Reflection;

namespace FairPlayCombined.Services.Common
{
    public class YouTubeClientService(
        YouTubeClientServiceConfiguration youTubeClientServiceConfiguration,
        IHttpContextAccessor httpContextAccessor)
    {
        public async Task UploadVideoAsync(Video video, Stream fileStream,
            Action<IUploadProgress> progressChanged,
            Action<Video> responseReceived)
        {
            var youtubeService = await AuthorizeAsync();

            var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
            videosInsertRequest.ProgressChanged += progressChanged;
            videosInsertRequest.ResponseReceived += responseReceived;

            await videosInsertRequest.UploadAsync();
        }

        public async Task<YouTubeService> AuthorizeAsync()
        {
            var userName = httpContextAccessor.HttpContext.User.Identity!.Name;
            UserCredential credential;
            using (var stream = new FileStream(youTubeClientServiceConfiguration.ClientSecretsFilePath!, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    [ YouTubeService.Scope.YoutubeUpload,
                    YouTubeService.Scope.YoutubeForceSsl,
                    YouTubeService.Scope.Youtubepartner],
                    userName,
                    CancellationToken.None
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });
            return youtubeService;
        }

        public static async Task<UploadStatus> UploadCaptionsAsync(string youtubeVideoId, string language,
            YouTubeService youTubeService,
            MemoryStream captionsStream,
            Action<IUploadProgress> progressChanged,
            Action<Caption> responseReceived,
            CancellationToken cancellationToken)
        {
            Caption caption = new()
            {
                Snippet = new CaptionSnippet()
            };
            caption.Snippet.Name = $"VI";
            caption.Snippet.Language = language;
            caption.Snippet.VideoId = youtubeVideoId;
            var uploadCaptionsOperation =
            youTubeService.Captions.Insert(body: caption, "snippet",
                captionsStream, "*/*");
            uploadCaptionsOperation.ProgressChanged += progressChanged;
            uploadCaptionsOperation.ResponseReceived += responseReceived;
            var result = await uploadCaptionsOperation.UploadAsync(cancellationToken);
            return result.Status;
        }
    }

    public class YouTubeClientServiceConfiguration
    {
        public string? ClientSecretsFilePath { get; set; }
    }
}
