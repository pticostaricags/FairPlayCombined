using FairPlayCombined.Models.Common.GeoLocation;

namespace FairPlayCombined.Interfaces
{
    public interface IGeoLocationService
    {
        Task<GeoCoordinates> GetCurrentPositionAsync();
    }
}
