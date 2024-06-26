using FairPlayCombined.Models.AzureVideoIndexer;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IAzureVideoIndexerService
    {
        Task<string> AuthenticateToAzureArmAsync();
        Task<bool> DeleteVideoByIdAsync(string videoId, string viAccessToken, CancellationToken cancellationToken);
        Task<GetAccessTokenResponseModel?> GetAccessTokenForArmAccountAsync(string bearerToken, CancellationToken cancellationToken);
        Task<SupportedLanguageModel[]?> GetSupportedLanguagesAsync(string viAccessToken, CancellationToken cancellationToken = default);
        Task<GetVideoIndexResponseModel?> GetVideoIndexAsync(string videoId, string viAccessToken, CancellationToken cancellationToken = default);
        Task<string> GetVideoStreamingUrlAsync(string videoId, string viAccessToken, CancellationToken cancellationToken = default);
        Task<byte[]?> GetVideoThumbnailAsync(string videoId, string thumbnailId, string viAccessToken, CancellationToken cancellationToken = default);
        Task<string> GetVideoVTTCaptionsAsync(string videoId, string viAccessToken, string language, CancellationToken cancellationToken = default);
        Task<UploadVideoResponseModel?> IndexVideoFromBytesAsync(IndexVideoFromBytesFormatModel indexVideoFromBase64FormatModel, string viAccountAccessToken, CancellationToken cancellationToken);
        Task<UploadVideoResponseModel?> IndexVideoFromUriAsync(IndexVideoFromUriParameters indexVideoFromUriParameters, CancellationToken cancellationToken = default);
        Task<SearchVideosResponseModel?> SearchVideosByIdsAsync(string viAccessToken, string[] videoIds, CancellationToken cancellationToken = default);
        Task<SearchVideosResponseModel?> SearchVideosByNameAsync(string viAccessToken, string name, CancellationToken cancellationToken = default);
        Task<string> GetFacesThumbnailsDownloadUrlAsync(
            string videoId, string viAccessToken, CancellationToken cancellationToken);
    }
}
