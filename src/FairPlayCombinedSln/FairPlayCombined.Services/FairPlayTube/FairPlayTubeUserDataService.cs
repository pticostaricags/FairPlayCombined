using FairPlayCombined.Common.CustomExceptions;
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
        public async Task<byte[]> GetMyVideoDataAsync(long videoInfoId, CancellationToken cancellationToken)
        {
            var userId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var videoInfoEntity =
                await dbContext.VideoInfo
                .AsNoTracking()
                .SingleAsync(p => p.VideoInfoId == videoInfoId, cancellationToken);
            if (videoInfoEntity.ApplicationUserId != userId)
            {
                throw new RuleException("You are not the owner of the specified video");
            }
            await using MemoryStream memoryStream = new();
            using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
            {
                var videosQuery = dbContext.VideoInfo
                    .Where(p => p.VideoInfoId == videoInfoId)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(p => p.VideoThumbnail)
                    .ThenInclude(p => p.Photo)
                    .Include(p => p.VideoInfographic)
                    .ThenInclude(p => p.Photo)
                    .Include(p => p.VideoDigitalMarketingPlan)
                    .Include(p => p.VideoDigitalMarketingDailyPosts)
                    .Include(p => p.VideoCaptions)
                    .Include(p => p.VideoKeyword)
                    .Include (p => p.VideoTopic)
                    .Where(p => p.ApplicationUserId == userId);

                if (await videosQuery.AnyAsync())
                {
                    foreach (var video in videosQuery)
                    {
                        await AddThumbnailsEntriesAsync(archive, video, cancellationToken);
                        await AddInfographicsEntriesAsync(archive, video, cancellationToken);
                        await AddDigitalMarketingPlanEntriesAsync(archive, video);
                        await AddDigitalMarketingDailyPostsEntriesAsync(archive, video);
                        await AddVideoCaptionsEntriesAsync(archive, video);
                        await AddVideoKeywordsEntriesAsync(archive, video);
                        await AddVideoTopicsEntriesAsync(archive, video);
                    }
                }
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
        }

        public async Task<byte[]> GetMyUserDataAsync(CancellationToken cancellationToken)
        {
            var userId = userProviderService.GetCurrentUserId();
            await using MemoryStream memoryStream = new();
            using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
            {
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var videosQuery = dbContext.VideoInfo
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(p => p.VideoThumbnail)
                    .ThenInclude(p => p.Photo)
                    .Include(p => p.VideoInfographic)
                    .ThenInclude(p => p.Photo)
                    .Include(p => p.VideoDigitalMarketingPlan)
                    .Include(p => p.VideoDigitalMarketingDailyPosts)
                    .Include(p => p.VideoCaptions)
                    .Include(p=>p.VideoKeyword)
                    .Include(p=>p.VideoTopic)
                    .Where(p => p.ApplicationUserId == userId);

                if (await videosQuery.AnyAsync())
                {
                    foreach (var video in videosQuery)
                    {
                        await AddThumbnailsEntriesAsync(archive, video, cancellationToken);
                        await AddInfographicsEntriesAsync(archive, video, cancellationToken);
                        await AddDigitalMarketingPlanEntriesAsync(archive, video);
                        await AddDigitalMarketingDailyPostsEntriesAsync(archive, video);
                        await AddVideoCaptionsEntriesAsync(archive, video);
                        await AddVideoKeywordsEntriesAsync(archive, video);
                        await AddVideoTopicsEntriesAsync(archive, video);
                    }
                }
                if (await dbContext.NewVideoRecommendation.AnyAsync(cancellationToken))
                {
                    foreach (var singleNewVideoRecommendation in dbContext.NewVideoRecommendation)
                    {
                        string fileName = @$"recommendations\{singleNewVideoRecommendation.NewVideoRecommendationId}.html";
                        var newVideoRecommendationEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                        await using var newVideoRecommendationEntryStream = newVideoRecommendationEntry.Open();
                        using StreamWriter streamWriter = new(newVideoRecommendationEntryStream);
                        await streamWriter.WriteLineAsync(singleNewVideoRecommendation.HtmlNewVideoRecommendation);
                    }
                }
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
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

        private static async Task AddDigitalMarketingPlanEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            if (video!.VideoDigitalMarketingPlan.Any())
            {
                foreach (var singleVideoDigitalMarketingPlan in video.VideoDigitalMarketingPlan)
                {
                    string fileName = @$"videos\{video.Name}\digitalmarketingplan\{singleVideoDigitalMarketingPlan.VideoDigitalMarketingPlan1}.html";
                    var videoDigitalMarketingPlanEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                    await using var videoDigitalMarketingPlanEntryArchiveEntryStream = videoDigitalMarketingPlanEntry.Open();
                    using StreamWriter streamWriter = new(videoDigitalMarketingPlanEntryArchiveEntryStream);
                    await streamWriter.WriteLineAsync(singleVideoDigitalMarketingPlan!.HtmlDigitalMarketingPlan);
                }
            }
        }

        private static async Task AddDigitalMarketingDailyPostsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            if (video!.VideoDigitalMarketingDailyPosts.Any())
            {
                foreach (var singleVideoDigitalMarketingDailyPost in video.VideoDigitalMarketingDailyPosts)
                {
                    string fileName = @$"videos\{video.Name}\digitalmarketingdailypost\{singleVideoDigitalMarketingDailyPost.VideoDigitalMarketingDailyPostsId}.html";
                    var videoDigitalMarketingDailyPostEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                    await using var videoDigitalMarketingDailyPostEntryStream = videoDigitalMarketingDailyPostEntry.Open();
                    using StreamWriter streamWriter = new(videoDigitalMarketingDailyPostEntryStream);
                    await streamWriter.WriteLineAsync(singleVideoDigitalMarketingDailyPost!.HtmlVideoDigitalMarketingDailyPostsIdeas);
                }
            }
        }

        private static async Task AddVideoCaptionsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            if (video!.VideoCaptions.Any())
            {
                foreach (var singleVideoCaption in video.VideoCaptions)
                {
                    string fileName = @$"videos\{video.Name}\videocaptions\{singleVideoCaption.Language}.vtt";
                    var videoCaptioEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                    await using var videoCaptioEntryStream = videoCaptioEntry.Open();
                    using StreamWriter streamWriter = new(videoCaptioEntryStream);
                    await streamWriter.WriteLineAsync(singleVideoCaption!.Content);
                }
            }
        }

        private static async Task AddVideoKeywordsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            if (video!.VideoKeyword.Any())
            {
                string keywords = String.Join(",", video.VideoKeyword.Select(p => p.Keyword));
                string fileName = @$"videos\{video.Name}\keywords.txt";
                var videoKeywordsEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                await using var videoKeywordsEntryStream = videoKeywordsEntry.Open();
                using StreamWriter streamWriter = new(videoKeywordsEntryStream);
                await streamWriter.WriteLineAsync(keywords);
            }
        }

        private static async Task AddVideoTopicsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            if (video!.VideoTopic.Any())
            {
                string topics = String.Join(",", video.VideoTopic.Select(p => p.Topic));
                string fileName = @$"videos\{video.Name}\topics.txt";
                var videoTopicsEntry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
                await using var videoTopicsEntryStream = videoTopicsEntry.Open();
                using StreamWriter streamWriter = new(videoTopicsEntryStream);
                await streamWriter.WriteLineAsync(topics);
            }
        }
    }
}
