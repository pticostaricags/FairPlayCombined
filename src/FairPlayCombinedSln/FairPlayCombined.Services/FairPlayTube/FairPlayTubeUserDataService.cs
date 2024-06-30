using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class FairPlayTubeUserDataService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService) : IFairPlayTubeUserDataService
    {
        public async Task<byte[]> GetMyUserDataAsync(CancellationToken cancellationToken)
        {
            var userId = userProviderService.GetCurrentUserId();
            await using MemoryStream memoryStream = new();
            using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
            {
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var videosQuery = dbContext.VideoInfo.AsNoTracking()
                    .AsSplitQuery()
                    .Include(p => p.VideoThumbnail)
                    .ThenInclude(p => p.Photo)
                    .Where(p => p.ApplicationUserId == userId);

                var videos = await videosQuery.ToListAsync(cancellationToken);

                if (videos.Any())
                {
                    foreach (var video in videos)
                    {
                        if (video.VideoThumbnail.Any())
                        {
                            foreach (var singleVideoThumbnail in video.VideoThumbnail)
                            {
                                string fileName = @$"{singleVideoThumbnail.VideoThumbnailId}.png";
                                var thumbnailArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                                await using var thumbnailArchiveEntryStream = thumbnailArchiveEntry.Open();
                                await thumbnailArchiveEntryStream
                                    .WriteAsync(singleVideoThumbnail.Photo.PhotoBytes, 0, singleVideoThumbnail.Photo.PhotoBytes.Length, cancellationToken);
                            }
                        }
                    }
                }
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
        }

    }
}
