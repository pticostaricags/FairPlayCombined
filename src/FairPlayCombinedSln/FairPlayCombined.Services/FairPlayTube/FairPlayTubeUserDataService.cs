using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
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
                    .Include(p=>p.VideoInfographic)
                    .ThenInclude(p=>p.Photo)
                    .Where(p => p.ApplicationUserId == userId);

                var videos = await videosQuery.ToListAsync(cancellationToken);

                if (videos.Any())
                {
                    foreach (var video in videos)
                    {
                        await AddThumbnailsEntriesAsync(archive, video, cancellationToken);
                        await AddInfographicsEntriesAsync(archive, video, cancellationToken);
                    }
                }
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
        }

        private static async Task AddInfographicsEntriesAsync(ZipArchive archive, VideoInfo? video, CancellationToken cancellationToken)
        {
            if (video!.VideoInfographic.Any())
            {
                foreach (var singleVideoInfographic in video.VideoInfographic)
                {
                    string fileName = @$"videos\{video.Name}\infographics\{singleVideoInfographic.VideoInfographicId}.png";
                    var infographicArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                    await using var infographicArchiveEntryStream = infographicArchiveEntry.Open();
                    await infographicArchiveEntryStream
                        .WriteAsync(singleVideoInfographic.Photo.PhotoBytes, 0, singleVideoInfographic.Photo.PhotoBytes.Length, cancellationToken);
                }
            }
        }

        private static async Task AddThumbnailsEntriesAsync(ZipArchive archive, VideoInfo? video, CancellationToken cancellationToken)
        {
            if (video!.VideoThumbnail.Any())
            {
                foreach (var singleVideoThumbnail in video.VideoThumbnail)
                {
                    string fileName = @$"videos\{video.Name}\thumbnails\{singleVideoThumbnail.VideoThumbnailId}.png";
                    var thumbnailArchiveEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                    await using var thumbnailArchiveEntryStream = thumbnailArchiveEntry.Open();
                    await thumbnailArchiveEntryStream
                        .WriteAsync(singleVideoThumbnail.Photo.PhotoBytes, 0, singleVideoThumbnail.Photo.PhotoBytes.Length, cancellationToken);
                }
            }
        }
    }
}
