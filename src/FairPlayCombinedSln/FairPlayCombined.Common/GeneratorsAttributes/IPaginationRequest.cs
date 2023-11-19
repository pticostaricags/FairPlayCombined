using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Common.GeneratorsAttributes
{
    public interface IPaginationRequest
    {
        ISortingItem[]? SortingItems { get; set; }
        int StartIndex { get; set; }
        int PageSize { get; set; }
    }

    public interface ISortingItem
    {
        string? PropertyName { get; set; }
        SortType SortType { get; set; }
    }

    public enum SortType
    {
        Ascending,
        Descending
    }
}
