namespace FairPlayCombined.Models.AzureMaps.SearchPOICategoryTree
{
    public static class SearchPOICategoryTreeResponseExtensions
    {
        public static List<POICategoryModel> ToPOICategoryModelList(this SearchPOICategoryTreeResponse source)
        {
            List<POICategoryModel> result = new();
            foreach (var singleParentCategory in source.poiCategories!)
            {
                POICategoryModel pOICategoryModel = new()
                {
                    Id = singleParentCategory.id,
                    Name = singleParentCategory.name,
                    Synonyms = singleParentCategory.synonyms,
                    Children = new()
                };
                foreach (var categoryChild in singleParentCategory.childCategoryIds!)
                {
                    var childCategoryInfo = source.poiCategories.SingleOrDefault(p => p.id == categoryChild!.Value);
                    if (childCategoryInfo != null)
                    {
                        POICategoryModel childCategoryModel = new()
                        {
                            Id = childCategoryInfo.id,
                            Name = childCategoryInfo.name,
                            Synonyms = childCategoryInfo.synonyms
                        };
                        pOICategoryModel.Children.Add(childCategoryModel);
                    }
                }
                result.Add(pOICategoryModel);
            }
            return result;
        }
    }
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
