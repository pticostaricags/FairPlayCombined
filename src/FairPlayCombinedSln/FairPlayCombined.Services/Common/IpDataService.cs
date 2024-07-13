using FairPlayCombined.Models.Common.IpData;
using System.Net;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class IpDataService(IpDataConfiguration ipDataConfiguration, HttpClient httpClient)
    {

        /// <summary>
        /// Gets the geo location info for the specified ip address
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<GetGeoLocationInfoResponse> GetIpGeoLocationInfoAsync(IPAddress ipAddress,
            CancellationToken cancellationToken = default)
        {
            string requestUrl =
                $"https://api.ipdata.co/{ipAddress}?api-key={ipDataConfiguration.Key}";
            var result = await httpClient
                .GetFromJsonAsync<GetGeoLocationInfoResponse>(requestUrl, cancellationToken);
            return result!;
        }
    }
}
