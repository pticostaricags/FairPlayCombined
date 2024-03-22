using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 // Naming Styles
namespace FairPlayCombined.Models.AzureVideoIndexer
{
    public class SearchVideosResponseModel
    {
        public Result[]? results { get; set; }
        public Nextpage? nextPage { get; set; }
    }

    public class Nextpage
    {
        public int pageSize { get; set; }
        public int skip { get; set; }
        public bool done { get; set; }
    }

    public class Result
    {
        public string? accountId { get; set; }
        public string? id { get; set; }
        public object? partition { get; set; }
        public object? externalId { get; set; }
        public object? metadata { get; set; }
        public string? name { get; set; }
        public object? description { get; set; }
        public string? created { get; set; }
        public string? lastModified { get; set; }
        public string? lastIndexed { get; set; }
        public string? privacyMode { get; set; }
        public string? userName { get; set; }
        public bool isOwned { get; set; }
        public bool isBase { get; set; }
        public string? state { get; set; }
        public string? processingProgress { get; set; }
        public int durationInSeconds { get; set; }
        public string? thumbnailVideoId { get; set; }
        public string? thumbnailId { get; set; }
        public Social? social { get; set; }
        public object[]? searchMatches { get; set; }
        public string? indexingPreset { get; set; }
        public string? streamingPreset { get; set; }
        public string? sourceLanguage { get; set; }
    }

    public class Social
    {
        public bool likedByUser { get; set; }
        public int likes { get; set; }
        public int views { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles