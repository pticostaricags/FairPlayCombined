using FairPlayCombined.Interfaces;

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
}