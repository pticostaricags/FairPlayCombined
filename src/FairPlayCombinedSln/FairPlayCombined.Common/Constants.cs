namespace FairPlayCombined.Common
{
    public static class Constants
    {
        public static class GeoCoordinates
        {
            /// <summary>
            /// 4326 refers to WGS 84, a standard used in GPS and other geographic systems.
            /// Check: https://learn.microsoft.com/en-us/ef/core/modeling/spatial
            /// </summary>
            public const int SRID = 4326;
        }
        public static class Matches
        {
            public const int MaxAllowedAgeDifference = 10;
        }
        public static class RoleName
        {
            public const string SystemAdmin = nameof(SystemAdmin);
            public const string User = nameof(User);
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
