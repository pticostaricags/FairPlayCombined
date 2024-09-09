using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.Contact
{
    public class UpdateContactModel : IUpdateModel
    {
        [DeniedValues(default(long))]
        public long ContactId { get; set; }
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
        [StringLength(50)]
        [Phone]
        public string? BusinessPhoneNumber { get; set; }
        [StringLength(50)]
        [Phone]
        public string? MobilePhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
    }
}
