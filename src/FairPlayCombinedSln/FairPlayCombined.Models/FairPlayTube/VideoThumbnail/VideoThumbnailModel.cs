using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
