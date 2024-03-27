namespace FairPlayCombined.Common.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ResourceKeyAttribute(string? defaultValue) : Attribute
    {
        public string? DefaultValue => defaultValue;
    }
}
