using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.Pagination
{
    public class PaginationOfT<T> : IPaginationOfT<T>
    {
        public int TotalItems { get; set; }
        public T[]? Items { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
