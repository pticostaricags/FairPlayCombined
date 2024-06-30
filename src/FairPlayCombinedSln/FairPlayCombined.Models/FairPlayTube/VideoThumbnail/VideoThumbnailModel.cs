using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.VideoThumbnail
{
    public class VideoThumbnailModel : IListModel
    {
        [DeniedValues(default(long))]
        public long VideoThumbnailId { get; set; }
        [DeniedValues(default(long))]
        public long VideoInfoId { get; set; }
        [DeniedValues(default(long))]
        public long PhotoId { get; set; }
        public byte[]? PhotoBytes { get; set; }
        public decimal ThumbnailCost { get; set; }
    }
}
