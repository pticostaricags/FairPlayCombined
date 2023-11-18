namespace FairPlayCombined.Common
{
    public class Constants
    {
        public class RoleName
        {
            const string SystemAdmin = nameof(SystemAdmin);
        }
        public static class Pagination
        {
            public const int PageSize = 10;
        }
        public static class CacheConfiguration
        {
            public static readonly TimeSpan LocalizationCacheDuration = TimeSpan.FromSeconds(5);
        }
    }
}
