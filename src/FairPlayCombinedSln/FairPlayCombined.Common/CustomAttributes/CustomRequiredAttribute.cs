using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.CustomAttributes
{
    [LocalizerOfT<CustomRequiredAttribute>]
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
