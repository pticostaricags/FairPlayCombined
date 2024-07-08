using FairPlayCombined.Models.Common.IpData;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class IpDataService(IpDataConfiguration ipDataConfiguration, 
        ILogger<IpDataService> logger, HttpClient httpClient)
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
            try
            {
                var result = await httpClient
                    .GetFromJsonAsync<GetGeoLocationInfoResponse>(requestUrl, cancellationToken);
                return result!;
            }
            catch (Exception ex)
            {
                logger.LogError("Error: {ErrorMessage}", ex.Message);
                throw;
            }
        }
    }
}
