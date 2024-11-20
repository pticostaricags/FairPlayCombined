namespace FairPlayCombined.Common.GeneratorsAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
#pragma warning disable S2326 // Unused type parameters should be removed
#pragma warning disable CS9113 // Parameter is unread.
    public class ModelOfEntityAttribute<T>(string entityNameWithSchema) : Attribute
#pragma warning restore CS9113 // Parameter is unread.
#pragma warning restore S2326 // Unused type parameters should be removed
        where T : ICreateModel
    {
    }
}
