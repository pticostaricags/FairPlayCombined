using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Common.ValidationAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
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
