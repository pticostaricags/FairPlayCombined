using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
