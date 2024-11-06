#pragma warning disable S101 // Types should be named in PascalCase
namespace FairPlayCombined.Models.AzureMaps.SearchPOICategoryTree
{
    public class POICategoryModel
    {
        public List<POICategoryModel>? Children { get; set; }
        public int Id { get; set; }
        public string? Name { get; set; }
        public string[]? Synonyms { get; set; }
    }
}
#pragma warning restore S101 // Types should be named in PascalCase