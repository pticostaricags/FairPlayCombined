namespace FairPlayCombined.Models.AzureVideoIndexer
{
    public class IndexVideoFromUriParameters
    {

        public Uri? VideoUri { get; set; }
        public string? ArmAccessToken { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? FileName { get; set; }
        public string? Language { get; set; } = "auto";
        public string? IndexingPreset { get; set; } = "Default";
    }
}