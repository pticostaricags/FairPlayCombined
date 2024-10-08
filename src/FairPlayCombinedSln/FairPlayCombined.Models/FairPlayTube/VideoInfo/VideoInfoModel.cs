﻿using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayTube.VideoInfo
{
    public class VideoInfoModel : IListModel
    {
        public long VideoInfoId { get; set; }
        public Guid AccountId { get; set; }
        public string? VideoId { get; set; }
        public string? Location { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public string? VideoBloblUrl { get; set; }
        public string? IndexedVideoUrl { get; set; }
        public string? ApplicationUserId { get; set; }
        public int VideoIndexStatusId { get; set; }
        public double VideoDurationInSeconds { get; set; }
        public string? VideoIndexSourceClass { get; set; }
        public decimal Price { get; set; }
        public string? ExternalVideoSourceUrl { get; set; }
        public string? VideoLanguageCode { get; set; }
        public int VideoVisibilityId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? YouTubeVideoId { get; set; }
        public string[]? VideoKeywords { get; set; }
        public string[]? VideoTopics { get; set; }
        public string? EnglishCaptions { get; set; }
        public int LifetimeSessions { get; set; }
        public int LifetimeViewers { get; set; }
        public TimeSpan LifetimeWatchTime { get; set; }
        public string? PublishedUrl { get; set; }
        public string? PublishedOnString { get; set; }
        public int? VideoIndexingProcessingPercentage { get; set; }
        public DateTimeOffset RowCreationDateTime { get; set; }
        public decimal IndexingCost { get; set; }
        public string AllInsightsUrl => $"https://www.videoindexer.ai/embed/insights/{AccountId}/{VideoId}/?&locale=en&location={Location}&controls=search,presets,language";
        public bool? IsVideoGeneratedWithAi { get; set; }
        public string VideoObjectDuration
        {
            get
            {
                var timeSpan = TimeSpan.FromSeconds(this.VideoDurationInSeconds);
                return $"PT{timeSpan.Hours}H{timeSpan.Minutes}M{timeSpan.Seconds}S";
            }
        }
        public string? GitHubSponsorsUsername { get; set; }
        public string? BuyMeACoffeeUsername { get; set; }
    }
}
