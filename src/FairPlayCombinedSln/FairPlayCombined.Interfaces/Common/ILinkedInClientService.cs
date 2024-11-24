using FairPlayCombined.Models.LinkedInApi;

namespace FairPlayCombined.Interfaces.Common
{
    public interface ILinkedInClientService
    {
        Task CreateTextShareAsync(string text, CancellationToken cancellationToken);
        Task<UserInfoModel> GetUserInfoAsync(CancellationToken cancellationToken);
        Task CreateArticleOrUrlShareAsync(string text, string? title,
            string? description, string url, CancellationToken cancellationToken);
        Task<bool> CreateImageShareAsync(string text,
            Stream imageStream, string filename,
            string mediaDescription, string mediaTitle,
            CancellationToken cancellationToken);
    }
}