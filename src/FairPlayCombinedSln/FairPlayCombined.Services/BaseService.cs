using FairPlayCombined.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services
{
    public abstract class BaseService
    {
        protected string GetSortTypeString(SortType sortType)
        {
            return sortType == SortType.Ascending ? "ASC" : "DESC";
        }
    }
}
