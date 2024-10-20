using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.ValidationAttributes;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace FairPlayCombined.Models.Auth.ExternalLogin
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
        [CustomStringLength(50)]
        [Display(
            ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_Name))]
        public string Name { get; set; } = "";

        [CustomRequired]
        [CustomStringLength(50)]
        [Display(
            ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_Lastname))]
        public string Lastname { get; set; } = "";

        [NullableUrl]
        [Display(
        ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_LinkedInProfileUrl))]
        public string? LinkedInProfileUrl { get; set; }

        [NullableUrl]
        [Display(
        ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_InstagramProfileUrl))]
        public string? InstagramProfileUrl { get; set; }

        [NullableUrl]
        [Display(
        ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_XFormerlyTwitterUrl))]
        public string? XformerlyTwitterUrl { get; set; }

        [NullableUrl]
        [Display(
        ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_WebsiteUrl))]
        public string? WebsiteUrl { get; set; }
    }

    [CompilerGenerated]
    [LocalizerOfT<InputModel>]
    public class InputModelLocalizer
    {
        public InputModelLocalizer() { }
        public static IStringLocalizer<InputModelLocalizer>? Localizer { get; set; }
        public static string? DisplayFor_Email => Localizer![DisplayFor_Email_TextKey];
        public static string? DisplayFor_Password => Localizer![DisplayFor_Password_TextKey];
        public static string? DisplayFor_ConfirmPassword => Localizer![DisplayFor_ConfirmPassword_TextKey];
        public static string? DisplayFor_Name => Localizer![DisplayFor_Name_TextKey];
        public static string? DisplayFor_Lastname => Localizer![DisplayFor_Lastname_TextKey];
        public static string? DisplayFor_LinkedInProfileUrl => Localizer![DisplayFor_LinkedInProfileUrlTextKey];
        public static string? DisplayFor_InstagramProfileUrl => Localizer![DisplayFor_InstagramProfileUrlTextKey];
        public static string? DisplayFor_XFormerlyTwitterUrl => Localizer![DisplayFor_XFormerlyTwitterUrlTextKey];
        public static string? DisplayFor_WebsiteUrl => Localizer![DisplayFor_WebsiteUrlTextKey];


        #region Resource Keys
        [ResourceKey(defaultValue: "Email")]
        public const string DisplayFor_Email_TextKey = "DisplayFor_Email_Text";
        [ResourceKey(defaultValue: "Password")]
        public const string DisplayFor_Password_TextKey = "DisplayFor_Password_Text";
        [ResourceKey(defaultValue: "Confirm password")]
        public const string DisplayFor_ConfirmPassword_TextKey = "DisplayFor_ConfirmPassword_Text";
        [ResourceKey(defaultValue: "Name")]
        public const string DisplayFor_Name_TextKey = "DisplayFor_Name_Text";
        [ResourceKey(defaultValue: "Lastname")]
        public const string DisplayFor_Lastname_TextKey = "DisplayFor_Lastname_Text";
        [ResourceKey(defaultValue: "LinkedIn Profile Url")]
        public const string DisplayFor_LinkedInProfileUrlTextKey = "DisplayFor_LinkedInProfileUrlText";
        [ResourceKey(defaultValue: "Instagram Profile Url")]
        public const string DisplayFor_InstagramProfileUrlTextKey = "DisplayFor_InstagramProfileUrlText";
        [ResourceKey(defaultValue: "X (formerly Twitter) Url")]
        public const string DisplayFor_XFormerlyTwitterUrlTextKey = "DisplayFor_XFormerlyTwitterUrlText";
        [ResourceKey(defaultValue: "Website Url")]
        public const string DisplayFor_WebsiteUrlTextKey = "DisplayFor_WebsiteUrlText";
        #endregion Resource Keys
    }
}
