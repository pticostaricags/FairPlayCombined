using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.LikedUserProfile
{
    public class LikedUserProfileModel : IListModel
    {
        public long? LikedUserProfileId { get; set; }
        public string? LikingApplicationUserId { get; set; }
        public string? LikedApplicationUserId { get; set; }
    }
}
