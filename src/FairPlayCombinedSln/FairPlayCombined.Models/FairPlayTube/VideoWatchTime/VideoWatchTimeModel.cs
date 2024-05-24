using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.VideoWatchTime
{
    public class VideoWatchTimeModel : IListModel
    {
        public long? VideoWatchTimeId { get; set; }
        [Required]
        [DeniedValues(default(long))]
        public long VideoInfoId { get; set; }
        [Required]
        public Guid? SessionId { get; set; } = Guid.Empty;
        [Required]
        public DateTimeOffset? SessionStartDatetime { get; set; }
        public double WatchTime { get; set; }
        [StringLength(450)]
        public string? WatchedByApplicationUserId { get; set; }
    }
}
