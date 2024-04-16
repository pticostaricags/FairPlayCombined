using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
