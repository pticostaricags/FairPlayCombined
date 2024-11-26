using FairPlayCombined.Models.LinkedInApi;

namespace FairPlayCombined.Interfaces.Common
{
    public interface ILinkedInClientService
    {
        Task<string> GetAccessTokenForUserAsync(string userId, CancellationToken cancellationToken);
        Task CreateTextShareAsync(string text, string accessToken, CancellationToken cancellationToken);
        Task<UserInfoModel> GetUserInfoAsync(string accessToken, CancellationToken cancellationToken);
        Task CreateArticleOrUrlShareAsync(string text, string accessToken, string? title,
            string? description, string url, CancellationToken cancellationToken);
        Task<bool> CreateImageShareAsync(string text,
            string accessToken,
            Stream imageStream, string filename,
            string mediaDescription, string mediaTitle,
            CancellationToken cancellationToken);
    }
}