using FairPlayCombined.Models.Common.GeoLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces
{
    public interface IGeoLocationService
    {
        Task<GeoCoordinates> GetCurrentPositionAsync();
    }
}
