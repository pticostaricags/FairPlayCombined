using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Common.CustomAttributes
{
    [LocalizerOfT<CustomCompareAttribute>]
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CustomCompareAttribute(string otherProperty) : CompareAttribute(otherProperty)
    {
        public override string FormatErrorMessage(string name)
        {
            var message = Localizer![PropertiesValuesDoNotMatchTextKey,
                this.OtherPropertyDisplayName ?? this.OtherProperty, name];
            return message;
        }
        public static IStringLocalizer<CustomCompareAttribute>? Localizer { get; set; }
        #region Resource Keys
        [ResourceKey(defaultValue: "{0} and {1} do not match")]
        public const string PropertiesValuesDoNotMatchTextKey = "PropertiesValuesDoNotMatchText";
        #endregion Resource Keys
    }
}
