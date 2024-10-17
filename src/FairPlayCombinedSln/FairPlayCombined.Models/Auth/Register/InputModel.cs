using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.ValidationAttributes;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FairPlayCombined.Models.Auth.Register
{
    public class InputModel
    {
        [CustomRequired]
        [CustomEmailAddress]
        [Display(
            ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_Email))]
        public string Email { get; set; } = "";

        [CustomRequired]
        [CustomStringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

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

    [CompilerGenerated]
    [LocalizerOfT<InputModel>]
    public class InputModelLocalizer
    {
        public static IStringLocalizer<InputModelLocalizer>? Localizer { get; set; }
        public static string? DisplayFor_Email => Localizer![DisplayFor_Email_TextKey];

        #region Resource Keys
        [ResourceKey(defaultValue: "Email")]
        public const string DisplayFor_Email_TextKey = "DisplayFor_Email_Text";
        #endregion Resource Keys
    }
}
