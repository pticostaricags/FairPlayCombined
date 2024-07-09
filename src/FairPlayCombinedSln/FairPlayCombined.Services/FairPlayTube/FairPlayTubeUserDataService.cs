using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.DataAccess.Models.FairPlayTubeSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.IO.Compression;

namespace FairPlayCombined.Services.FairPlayTube
{
    public class FairPlayTubeUserDataService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService,
        ILogger<FairPlayTubeUserDataService> logger) : IFairPlayTubeUserDataService
    {
        private const CompressionLevel DefaultCompressionLevel = CompressionLevel.SmallestSize;

        public async Task<byte[]> GetMyVideoDataAsync(long videoInfoId, CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetMyVideoDataAsync));
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
                    .Include(p => p.VideoDigitalMarketingPlan)
                    .Include(p => p.VideoDigitalMarketingDailyPosts)
                    .Include(p => p.VideoCaptions)
                    .Include(p => p.VideoKeyword)
                    .Include(p => p.VideoTopic)
                    .Where(p => p.ApplicationUserId == userId);

                if (await videosQuery.AnyAsync())
                {
                    foreach (var video in videosQuery)
                    {
                        await AddThumbnailsEntriesAsync(archive, video, dbContext, cancellationToken);
                        await AddInfographicsEntriesAsync(archive, video, dbContext, cancellationToken);
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

        public async Task<byte[]> GetUserDataAsync(string userId, CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(GetUserDataAsync));
            await using MemoryStream memoryStream = new();
            using (ZipArchive archive = new(memoryStream, ZipArchiveMode.Create, true))
            {
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var videosQuery = dbContext.VideoInfo
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(p => p.VideoDigitalMarketingPlan)
                    .Include(p => p.VideoDigitalMarketingDailyPosts)
                    .Include(p => p.VideoCaptions)
                    .Include(p => p.VideoKeyword)
                    .Include(p => p.VideoTopic)
                    .Where(p => p.ApplicationUserId == userId);

                if (await videosQuery.AnyAsync())
                {
                    foreach (var video in videosQuery)
                    {
                        await AddThumbnailsEntriesAsync(archive, video, dbContext, cancellationToken);
                        await AddInfographicsEntriesAsync(archive, video, dbContext, cancellationToken);
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
                        var newVideoRecommendationEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                        await using var newVideoRecommendationEntryStream = newVideoRecommendationEntry.Open();
                        using StreamWriter streamWriter = new(newVideoRecommendationEntryStream);
                        await streamWriter.WriteLineAsync(singleNewVideoRecommendation.HtmlNewVideoRecommendation);
                    }
                }
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream.ToArray();
        }

        private async Task AddThumbnailsEntriesAsync(ZipArchive archive, VideoInfo video,
            FairPlayCombinedDbContext dbContext, CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(AddThumbnailsEntriesAsync));
            var thumbnailsQuery = dbContext.VideoThumbnail
                .AsNoTracking()
                .Where(p => p.VideoInfoId == video.VideoInfoId);
            if (await thumbnailsQuery.AnyAsync(cancellationToken))
            {
                foreach (var singleVideoThumbnail in thumbnailsQuery)
                {
                    Photo photo = await dbContext.Photo.AsNoTracking()
                        .SingleAsync(p => p.PhotoId == singleVideoThumbnail.PhotoId,
                        cancellationToken);
                    string fileName = @$"videos\{video.Name}\thumbnails\{singleVideoThumbnail.VideoThumbnailId}.png";
                    var thumbnailArchiveEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                    await using var thumbnailArchiveEntryStream = thumbnailArchiveEntry.Open();
                    await thumbnailArchiveEntryStream
                        .WriteAsync(photo.PhotoBytes, 0, photo.PhotoBytes.Length, cancellationToken);
                }
            }
        }

        private async Task AddInfographicsEntriesAsync(ZipArchive archive, 
            VideoInfo video, FairPlayCombinedDbContext dbContext, 
            CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(AddInfographicsEntriesAsync));
            var infographicsQuery = dbContext.VideoInfographic
                .AsNoTracking()
                .Where(p => p.VideoInfoId == video.VideoInfoId);
            if (await infographicsQuery.AnyAsync(cancellationToken))
            {
                foreach (var singleVideoInfographic in infographicsQuery)
                {
                    var photo = await dbContext.Photo
                        .SingleAsync(p=>p.PhotoId == singleVideoInfographic!.PhotoId,
                        cancellationToken);
                    string fileName = @$"videos\{video.Name}\infographics\{singleVideoInfographic.VideoInfographicId}.png";
                    var infographicArchiveEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                    await using var infographicArchiveEntryStream = infographicArchiveEntry.Open();
                    await infographicArchiveEntryStream
                        .WriteAsync(photo.PhotoBytes, 0, photo.PhotoBytes.Length, cancellationToken);
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
                    var videoDigitalMarketingPlanEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                    await using var videoDigitalMarketingPlanEntryArchiveEntryStream = videoDigitalMarketingPlanEntry.Open();
                    using StreamWriter streamWriter = new(videoDigitalMarketingPlanEntryArchiveEntryStream);
                    await streamWriter.WriteLineAsync(singleVideoDigitalMarketingPlan!.HtmlDigitalMarketingPlan);
                }
            }
        }

        private async Task AddDigitalMarketingDailyPostsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(AddDigitalMarketingDailyPostsEntriesAsync));
            if (video!.VideoDigitalMarketingDailyPosts.Any())
            {
                foreach (var singleVideoDigitalMarketingDailyPost in video.VideoDigitalMarketingDailyPosts)
                {
                    string fileName = @$"videos\{video.Name}\digitalmarketingdailypost\{singleVideoDigitalMarketingDailyPost.VideoDigitalMarketingDailyPostsId}.html";
                    var videoDigitalMarketingDailyPostEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                    await using var videoDigitalMarketingDailyPostEntryStream = videoDigitalMarketingDailyPostEntry.Open();
                    using StreamWriter streamWriter = new(videoDigitalMarketingDailyPostEntryStream);
                    await streamWriter.WriteLineAsync(singleVideoDigitalMarketingDailyPost!.HtmlVideoDigitalMarketingDailyPostsIdeas);
                }
            }
        }

        private async Task AddVideoCaptionsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(AddVideoCaptionsEntriesAsync));
            if (video!.VideoCaptions.Any())
            {
                foreach (var singleVideoCaption in video.VideoCaptions)
                {
                    string fileName = @$"videos\{video.Name}\videocaptions\{singleVideoCaption.Language}.vtt";
                    var videoCaptioEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                    await using var videoCaptioEntryStream = videoCaptioEntry.Open();
                    using StreamWriter streamWriter = new(videoCaptioEntryStream);
                    await streamWriter.WriteLineAsync(singleVideoCaption!.Content);
                }
            }
        }

        private async Task AddVideoKeywordsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(AddVideoKeywordsEntriesAsync));
            if (video!.VideoKeyword.Any())
            {
                string keywords = String.Join(",", video.VideoKeyword.Select(p => p.Keyword));
                string fileName = @$"videos\{video.Name}\keywords.txt";
                var videoKeywordsEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                await using var videoKeywordsEntryStream = videoKeywordsEntry.Open();
                using StreamWriter streamWriter = new(videoKeywordsEntryStream);
                await streamWriter.WriteLineAsync(keywords);
            }
        }

        private async Task AddVideoTopicsEntriesAsync(ZipArchive archive, VideoInfo? video)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(AddVideoTopicsEntriesAsync));
            if (video!.VideoTopic.Any())
            {
                string topics = String.Join(",", video.VideoTopic.Select(p => p.Topic));
                string fileName = @$"videos\{video.Name}\topics.txt";
                var videoTopicsEntry = archive.CreateEntry(fileName, DefaultCompressionLevel);
                await using var videoTopicsEntryStream = videoTopicsEntry.Open();
                using StreamWriter streamWriter = new(videoTopicsEntryStream);
                await streamWriter.WriteLineAsync(topics);
            }
        }

        public async Task EnqueueMyDataExportAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(message: "Start of method: {MethodName}", nameof(EnqueueMyDataExportAsync));
            var currentUserId = userProviderService.GetCurrentUserId();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            if (await dbContext.UserDataExportQueue
                .AsNoTracking()
                .Where(p=>p.ApplicationUserId == currentUserId && 
                p.IsCompleted)
                .AnyAsync(cancellationToken))
            {
                throw new RuleException("There is already a request being processed");
            }
            await dbContext.UserDataExportQueue.AddAsync(new()
            {
                ApplicationUserId = currentUserId,
                IsCompleted = false
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
