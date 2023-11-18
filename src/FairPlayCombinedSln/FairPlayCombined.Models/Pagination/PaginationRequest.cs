using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Pagination
{
    public class PaginationRequest
    {
        public int StartIndex { get; set; }
        public SortingItem[]? SortingItems { get; set; }
    }

    public class SortingItem
    {
        public string? PropertyName { get; set; }
        public SortType SortType { get; set; }
    }

    public enum SortType
    {
        Ascending,
        Descending
    }
}
