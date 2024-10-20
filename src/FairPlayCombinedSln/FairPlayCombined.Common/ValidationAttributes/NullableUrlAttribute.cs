using FairPlayCombined.Common.CustomAttributes;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.ValidationAttributes
{
    [LocalizerOfT<NullableUrlAttribute>]
    [AttributeUsage(AttributeTargets.Property)]
    public class NullableUrlAttribute : ValidationAttribute
    {
        public static IStringLocalizer<NullableUrlAttribute>? Localizer {  get; set; }
        public override string FormatErrorMessage(string name)
        {
            var message = Localizer![InvalidUrlTextKey, name];
            return message;
        }

        public override bool IsValid(object? value)
        {
            if (value is null)
                return true;
            else
            {
                if (String.IsNullOrWhiteSpace(value!.ToString()))
                    return true;
                try
                {
                    if (Uri.IsWellFormedUriString(value!.ToString()!, UriKind.Absolute))
                        return true;
                    else
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "The field {0} is invalid.")]
        public const string InvalidUrlTextKey = "InvalidUrlText";
        #endregion Resource Keys
    }
}
