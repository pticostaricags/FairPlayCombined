using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.Pagination
{

    public class PaginationRequest : IPaginationRequest
    {
        public int StartIndex { get; set; }
        public ISortingItem[]? SortingItems { get; set; }
        public int PageSize { get; set; }
    }

    public class SortingItem : ISortingItem
    {
        public string? PropertyName { get; set; }
        public SortType SortType { get; set; }
    }
}
