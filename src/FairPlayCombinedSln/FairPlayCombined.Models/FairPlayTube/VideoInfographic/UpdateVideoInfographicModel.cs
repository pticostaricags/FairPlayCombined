using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.VideoInfographic
{
    public class UpdateVideoInfographicModel : IUpdateModel
    {

        [DeniedValues(default(long))]
        public long VideoInfographicId { get; set; }
        [DeniedValues(default(long))]
        public long VideoInfoId { get; set; }
        [DeniedValues(default(long))]
        public long PhotoId { get; set; }
    }
}
