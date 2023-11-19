using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
