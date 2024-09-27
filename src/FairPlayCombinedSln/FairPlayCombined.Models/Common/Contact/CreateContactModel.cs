using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Contact
{
    public class CreateContactModel : ICreateModel
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? Lastname { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string? EmailAddress { get; set; }
        [StringLength(50)]
        [Url]
        public string? LinkedInProfileUrl { get; set; }
        [StringLength(50)]
        [Url]
        public string? YouTubeChannelUrl { get; set; }
        [Url]
        public string? InstagramUrl { get; set; }
        [StringLength(50)]
        [Phone]
        public string? BusinessPhoneNumber { get; set; }
        [StringLength(50)]
        [Phone]
        public string? MobilePhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        [Required]
        [StringLength(450)]
        public string? OwnerApplicationUserId { get; set; }
    }
}
