using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.VideoDigitalMarketingDailyPosts
{
    public class VideoDigitalMarketingDailyPostsModel : IListModel
    {
        public long VideoDigitalMarketingDailyPostsId { get; set; }

        public long VideoInfoId { get; set; }

        public string? SocialNetworkName { get; set; }
        public string? HtmlVideoDigitalMarketingDailyPostsIdeas { get; set; }
    }
}
