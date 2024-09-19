namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IVideoPassiveIncomeIdeaService
    {
        Task<string> CreateVideoPassiveIncomeIdeaAsync(long videoInfoId, string languageCode,
            CancellationToken cancellationToken);
        Task<string[]?> GetVideoPassiveIncomeIdeasAsync(long videoInfoId, CancellationToken cancellationToken);
    }
}
