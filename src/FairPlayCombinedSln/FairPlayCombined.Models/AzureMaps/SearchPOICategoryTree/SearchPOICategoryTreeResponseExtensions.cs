#pragma warning disable S101 // Types should be named in PascalCase
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
}
#pragma warning restore S101 // Types should be named in PascalCase
