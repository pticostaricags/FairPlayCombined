using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Services
{
    public abstract class BaseService
    {
        protected static string GetSortTypeString(SortType sortType)
        {
            return sortType == SortType.Ascending ? "ASC" : "DESC";
        }
    }
}
