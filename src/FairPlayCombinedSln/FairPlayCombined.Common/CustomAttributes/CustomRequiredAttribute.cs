using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Common.CustomAttributes
{
    [LocalizerOfT<CustomRequiredAttribute>]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class CustomRequiredAttribute : RequiredAttribute
    {
        public static IStringLocalizer<CustomRequiredAttribute>? Localizer { get; set; }
        public override string FormatErrorMessage(string name)
        {
            var message = Localizer![RequiredTextKey, name];
            return message;
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "{0} is required")]
        public const string RequiredTextKey = "RequiredText";
        #endregion Resource Keys
    }
}
