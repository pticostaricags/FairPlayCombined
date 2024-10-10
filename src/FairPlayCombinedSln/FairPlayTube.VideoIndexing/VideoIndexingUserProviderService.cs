using FairPlayCombined.Interfaces;

namespace FairPlayTube.VideoIndexing;
internal class VideoIndexingUserProviderService : IUserProviderService
{
    public string? GetAccessToken()
    {
        throw new NotImplementedException();
    }

    public string? GetCurrentUserId()
    {
        return nameof(VideoIndexingUserProviderService);
    }

    public bool IsAuthenticatedWithGoogle()
    {
        throw new NotImplementedException();
    }

    public bool IsAuthenticatedWithLinkedIn()
    {
        throw new NotImplementedException();
    }
}