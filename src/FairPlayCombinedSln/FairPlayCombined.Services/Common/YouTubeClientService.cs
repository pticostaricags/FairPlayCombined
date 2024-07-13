using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.GoogleAuth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace FairPlayCombined.Services.Common
{
    public class YouTubeClientService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService) : IYouTubeClientService
    {
        public async Task<SearchListResponse> SearchMyVideosAsync(string searchTerm, CancellationToken cancellationToken)
        {
            var youtubeService = await AuthorizeAsync();
            var searchVideosRequest =
            youtubeService.Search.List(part: new(["snippet"]));
            searchVideosRequest.ForMine = true;
            searchVideosRequest.Type = "video";
            searchVideosRequest.Q = searchTerm;
            searchVideosRequest.Order = SearchResource.ListRequest.OrderEnum.Relevance;
            searchVideosRequest.MaxResults = 100;
            var result = await searchVideosRequest.ExecuteAsync(cancellationToken);
            return result;
        }
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
            var userId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync();
            var accessTokenEntity = await
                dbContext.AspNetUserTokens
                .SingleAsync(p => p.UserId == userId && p.LoginProvider == "Google" &&
                p.Name == "access_token");
            var credential = GoogleCredential.FromAccessToken(accessTokenEntity.Value);

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });
            return youtubeService;
        }

        public async Task<UploadStatus> UploadCaptionsAsync(string youtubeVideoId, string language,
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
        public GoogleAuthClientSecretInfo? GoogleAuthClientSecretInfo { get; set; }
    }
}
