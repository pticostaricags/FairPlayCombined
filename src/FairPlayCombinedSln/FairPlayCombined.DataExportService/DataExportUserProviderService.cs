using FairPlayCombined.Interfaces;

namespace FairPlayCombined.DataExportService;
internal class DataExportUserProviderService : IUserProviderService
{
    public string? GetAccessToken()
    {
        throw new NotImplementedException();
    }

    public string? GetCurrentUserId()
    {
        return nameof(DataExportUserProviderService);
    }

    public bool IsAuthenticatedWithGoogle()
    {
        throw new NotImplementedException();
    }
}