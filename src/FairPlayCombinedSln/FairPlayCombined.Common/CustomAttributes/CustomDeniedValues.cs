using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter,
        AllowMultiple = false)]
    public class CustomDeniedValuesAttribute(params object?[] values) : DeniedValuesAttribute(values)
    {
        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }
}
