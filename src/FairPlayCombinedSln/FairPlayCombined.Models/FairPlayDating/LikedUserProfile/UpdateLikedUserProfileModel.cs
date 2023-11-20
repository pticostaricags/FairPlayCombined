using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.LikedUserProfile
{
    public class UpdateLikedUserProfileModel : IUpdateModel
    {
        [Required]
        public long? LikedUserProfileId { get; set; }

        [Required]
        public string? LikingApplicationUserId { get; set; }

        [Required]
        public string? LikedApplicationUserId { get; set; }
    }
}
