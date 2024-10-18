using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.ValidationAttributes;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        [Display(
            ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_Password))]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(
            ResourceType = typeof(InputModelLocalizer),
            Name = nameof(InputModelLocalizer.DisplayFor_ConfirmPassword))]
        [CustomCompare(nameof(Password))]
        public string ConfirmPassword { get; set; } = "";

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
        public InputModelLocalizer() { }
        public static IStringLocalizer<InputModelLocalizer>? Localizer { get; set; }
        public static string? DisplayFor_Email => Localizer![DisplayFor_Email_TextKey];
        public static string? DisplayFor_Password => Localizer![DisplayFor_Password_TextKey];
        public static string? DisplayFor_ConfirmPassword => Localizer![DisplayFor_ConfirmPassword_TextKey];
        public static string? DisplayFor_Name => Localizer![DisplayFor_Name_TextKey];
        public static string? DisplayFor_Lastname => Localizer![DisplayFor_Lastname_TextKey];
        public static string? DisplayFor_LinkedInProfileUrl => Localizer![DisplayFor_LinkedInProfileUrlTextKey];
        
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
        #endregion Resource Keys
    }
}
