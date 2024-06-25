using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.VideoInfographic
{
    public class VideoInfographicModel : IListModel
    {

        public long VideoInfographicId { get; set; }
        public long VideoInfoId { get; set; }
        public long PhotoId { get; set; }
    }
}
