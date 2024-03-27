namespace FairPlayCombined.Interfaces
{
    public interface ICultureService
    {
        Task<string[]> GetSupportedCultures(CancellationToken cancellationToken);
    }
}
