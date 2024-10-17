using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.CustomAttributes
{
    [LocalizerOfT<CustomStringLengthAttribute>]
    public class CustomStringLengthAttribute(int maximumLength) : StringLengthAttribute(maximumLength)
    {
        public static IStringLocalizer<CustomStringLengthAttribute>? Localizer { get; set; }

        public override string FormatErrorMessage(string name)
        {
            var message = Localizer![InvalidLengthTextKey, name, this.MinimumLength, this.MaximumLength];
            return message;
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "The {0} must be at least {2} and at max {1} characters long.")]
        public const string InvalidLengthTextKey = "InvalidLengthText";
        #endregion Resource Keys
    }
}