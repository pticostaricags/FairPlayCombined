using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Auth.ExternalLogin
{
    public class InputModel
    {
        [CustomRequired]
        [CustomEmailAddress]
        public string Email { get; set; } = "";
        [CustomRequired]
        [CustomStringLength(50)]
        [Display(Name = "Name")]
        public string Name { get; set; } = "";

        [CustomRequired]
        [CustomStringLength(50)]
        [Display(Name = "Lastname")]
        public string Lastname { get; set; } = "";

        [NullableUrl]
        [Display(Name = "LinkedIn Profile Url")]
        public string? LinkedInProfileUrl { get; set; }

        [NullableUrl]
        [Display(Name = "Instagram Profile Url")]
        public string? InstagramProfileUrl { get; set; }

        [NullableUrl]
        [Display(Name = "X (formerly Twitter) Url")]
        public string? XformerlyTwitterUrl { get; set; }

        [NullableUrl]
        [Display(Name = "Website Url")]
        public string? WebsiteUrl { get; set; }
    }
}
