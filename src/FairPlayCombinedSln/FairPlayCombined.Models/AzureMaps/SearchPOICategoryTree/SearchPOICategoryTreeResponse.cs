#pragma warning disable S101 // Types should be named in PascalCase
namespace FairPlayCombined.Models.AzureMaps.SearchPOICategoryTree
{
    public class SearchPOICategoryTreeResponse
    {
        public Poicategory[]? poiCategories { get; set; }
    }

    public class Poicategory
    {
        public int id { get; set; }
        public string? name { get; set; }
        public int?[]? childCategoryIds { get; set; }
        public string[]? synonyms { get; set; }
    }

}
#pragma warning restore S101 // Types should be named in PascalCase