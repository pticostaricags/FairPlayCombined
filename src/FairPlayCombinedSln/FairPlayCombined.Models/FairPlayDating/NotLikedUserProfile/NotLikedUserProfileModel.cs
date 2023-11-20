using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.NotLikedUserProfile
{
    public class NotLikedUserProfileModel : IListModel
    {
        public long? NotLikedUserProfileId { get; set; }
        public string? NotLikingApplicationUserId { get; set; }
        public string? NotLikedApplicationUserId { get; set; }
    }
}
