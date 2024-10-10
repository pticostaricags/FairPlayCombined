namespace FairPlayCombined.Interfaces.Common
{
    public interface ILinkedInConnectionService
    {
        Task ImportFromConnectionsFileAsync(Stream stream, CancellationToken cancellationToken);
    }
}
