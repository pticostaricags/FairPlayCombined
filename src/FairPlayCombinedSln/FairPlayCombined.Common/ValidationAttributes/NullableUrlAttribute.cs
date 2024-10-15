using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NullableUrlAttribute : ValidationAttribute
    {
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
                    Uri uri = new(value!.ToString()!);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
