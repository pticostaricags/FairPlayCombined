using FairPlayCombined.Models.InstagramApi;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IInstagramClientService
    {
        Task<UserInfoModel?> GetUserInfoAsync(string username, string accessToken,
            CancellationToken cancellationToken);
        Task<UserMediaModel?> GetUserMediaAsync(string username, string accessToken,
            CancellationToken cancellationToken);
        Task<PublishMediaContainerResponseModel?> CreateSingleMediaPostAsync(string username, string accessToken,
            string imageUrl, CancellationToken cancellationToken);
        Task<PublishMediaContainerResponseModel?> CreateReelPostAsync(string username, string accessToken,
            string videoUrl, CancellationToken cancellationToken);
        Task<PublishMediaContainerResponseModel?>
            CreateImageStoryPostAsync(string username, string accessToken,
            string imageUrl, CancellationToken cancellationToken);
    }
}
