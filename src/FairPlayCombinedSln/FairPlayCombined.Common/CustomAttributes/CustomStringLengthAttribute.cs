using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
namespace FairPlayCombined.Common.CustomAttributes
{
    [LocalizerOfT<CustomStringLengthAttribute>]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class CustomStringLengthAttribute(int maximumLength) : StringLengthAttribute(maximumLength)
    {
        public static IStringLocalizer<CustomStringLengthAttribute>? Localizer { get; set; }

        public override string FormatErrorMessage(string name)
        {
            var message = Localizer![InvalidLengthTextKey, name, this.MinimumLength, this.MaximumLength];
            return message;
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "The {0} must be at least {1} and at max {2} characters long.")]
        public const string InvalidLengthTextKey = "InvalidLengthText";
        #endregion Resource Keys
    }
}