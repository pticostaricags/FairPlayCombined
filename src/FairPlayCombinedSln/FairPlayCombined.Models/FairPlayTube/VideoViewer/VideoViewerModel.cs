namespace FairPlayCombined.Models.FairPlayTube.VideoViewer
{
    public class VideoViewerModel
    {
        public string? VideoId { get; set; }
        public string? VideoName { get; set; }
        public string? Username { get; set; }
        public long TotalSessions { get; set; }
        public double TotalTimeWatched { get; set; }
    }
}
