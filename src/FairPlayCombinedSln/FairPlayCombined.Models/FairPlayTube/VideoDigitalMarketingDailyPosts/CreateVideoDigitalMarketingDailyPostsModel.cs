using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.VideoDigitalMarketingDailyPosts
{
    public class CreateVideoDigitalMarketingDailyPostsModel: ICreateModel
    {
        [DeniedValues(default(long))]
        public long VideoDigitalMarketingDailyPostsId { get; set; }
        [DeniedValues(default(long))]
        public long VideoInfoId { get; set; }

        [Required]
        [StringLength(50)]
        public string? SocialNetworkName { get; set; }

        [Required]
        public string? HtmlVideoDigitalMarketingDailyPostsIdeas { get; set; }
    }
}
