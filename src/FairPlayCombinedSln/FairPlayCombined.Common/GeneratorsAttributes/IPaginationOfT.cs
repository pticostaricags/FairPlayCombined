namespace FairPlayCombined.Common.GeneratorsAttributes
{
    public interface IPaginationOfT<T>
    {
        T[]? Items { get; set; }
        int PageSize { get; set; }
        int TotalItems { get; set; }
        int TotalPages { get; set; }
    }
}
