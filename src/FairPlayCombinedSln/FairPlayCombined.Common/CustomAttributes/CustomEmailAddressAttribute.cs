using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Common.CustomAttributes
{
    [LocalizerOfT<CustomEmailAddressAttribute>]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class CustomEmailAddressAttribute : ValidationAttribute
    {
        public static IStringLocalizer<CustomEmailAddressAttribute>? Localizer { get; set; }
        public override string FormatErrorMessage(string name)
        {
            var error = Localizer![InvalidEmailAddressTextKey, name];
            return error;
        }
        public override bool IsValid(object? value)
        {
            EmailAddressAttribute emailAddressAttribute = new();
            var isValid = emailAddressAttribute.IsValid(value);
            return isValid;
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "{0} must have a valid Email Address format")]
        public const string InvalidEmailAddressTextKey = "InvalidEmailAddressText";
        #endregion Resource Keys
    }
}
