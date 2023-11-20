using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.NotLikedUserProfile
{
    public class UpdateNotLikedUserProfileModel : IUpdateModel
    {
        [Required]
        public long? NotLikedUserProfileId { get; set; }

        [Required]
        public string? NotLikingApplicationUserId { get; set; }

        [Required]
        public string? NotLikedApplicationUserId { get; set; }
    }
}
