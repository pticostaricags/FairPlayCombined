namespace FairPlayCombined.Interfaces
{
    public interface IUserProviderService
    {
        string? GetCurrentUserId();
        string? GetAccessToken();
    }
}
