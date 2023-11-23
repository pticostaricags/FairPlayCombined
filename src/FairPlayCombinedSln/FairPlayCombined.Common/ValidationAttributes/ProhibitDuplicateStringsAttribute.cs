using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.ValidationAttributes
{
    public class ProhibitDuplicateStringsAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            var requiredType = typeof(string?[]).Name;
            if (value is null) return true;
            if (value is not string?[])
                throw new ValidationException($"Value must implement {requiredType}");
            var groupedItems = (value as string?[])!.GroupBy(p => p);
            if (groupedItems.Any(p => p.Count(x => !String.IsNullOrWhiteSpace(x)) > 1)) return false;
            return true;
        }
    }
}
