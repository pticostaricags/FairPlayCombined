namespace FairPlayCombined.Models.AzureVideoIndexer
{
    public class UploadVideoResponseModel
    {
        public string? accountId { get; set; }
        public string? id { get; set; }
        public object? partition { get; set; }
        public object? externalId { get; set; }
        public object? metadata { get; set; }
        public string? name { get; set; }
        public string? description { get; set; }
        public DateTime created { get; set; }
        public DateTime lastModified { get; set; }
        public DateTime lastIndexed { get; set; }
        public string? privacyMode { get; set; }
        public string? userName { get; set; }
        public bool isOwned { get; set; }
        public bool isBase { get; set; }
        public bool hasSourceVideoFile { get; set; }
        public string? state { get; set; }
        public string? moderationState { get; set; }
        public string? reviewState { get; set; }
        public object? processingProgress { get; set; }
        public int durationInSeconds { get; set; }
        public string? thumbnailVideoId { get; set; }
        public string? thumbnailId { get; set; }
        public object[]? searchMatches { get; set; }
        public string? indexingPreset { get; set; }
        public string? streamingPreset { get; set; }
        public string? sourceLanguage { get; set; }
        public string[]? sourceLanguages { get; set; }
        public string? personModelId { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles