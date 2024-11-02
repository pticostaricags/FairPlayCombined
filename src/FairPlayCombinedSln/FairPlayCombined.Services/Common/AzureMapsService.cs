using FairPlayCombined.Models.AzureMaps;
using FairPlayCombined.Models.AzureMaps.Enums;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class AzureMapsService(AzureMapsConfiguration azureMapsConfiguration,
        HttpClient httpClient)
    {
        public async Task<SearchByPOICategoryResponse> GetNearbyAirportsAsync(double latitude, double longitude, CancellationToken cancellationToken)
        {
            string format = "json";
            string query = POICategories.AIRPORT.ToString();
            string requestUrl = $"{azureMapsConfiguration.Endpoint}" +
                $"/search/poi/category/{format}" +
                $"?api-version=1.0" +
                $"&lat={latitude}" +
                $"&lon={longitude}" +
                $"&query={query}" +
                $"&subscription-key={azureMapsConfiguration.Key}";
            var result = await httpClient.GetFromJsonAsync<SearchByPOICategoryResponse>(requestUrl, cancellationToken);
            return result!;
        }
    }
}
