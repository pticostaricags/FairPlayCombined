using FairPlayCombined.Models.AzureMaps;
using FairPlayCombined.Models.AzureMaps.Enums;
using FairPlayCombined.Models.AzureMaps.GetNearbyPointsOfInterest;
using FairPlayCombined.Models.AzureMaps.SearchByPOICategory;
using FairPlayCombined.Models.AzureMaps.SearchPOICategoryTree;
using Microsoft.SqlServer.Server;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common
{
    public class AzureMapsService(AzureMapsConfiguration azureMapsConfiguration,
        HttpClient httpClient)
    {

        public async Task<SearchPOICategoryTreeResponse> GetPointsOfInterestCategoryTreeAsync(CancellationToken cancellationToken)
        {
            string format = "json";
            string requestUrl = $"https://atlas.microsoft.com/search/poi/category/tree/{format}" +
                $"?api-version=1.0" +
                $"&subscription-key={azureMapsConfiguration.Key}";
            var result = await httpClient.GetFromJsonAsync<SearchPOICategoryTreeResponse>(requestUrl, cancellationToken);
            return result!;
        }

        /// <summary>
        /// Check https://learn.microsoft.com/en-us/rest/api/maps/search/get-search-nearby?view=rest-maps-1.0&tabs=HTTP
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<SearchPointsOfInterestResponse> GetNearbyPointsOfInterestAsync(double latitude, double longitude, CancellationToken cancellationToken)
        {
            string format = "json";
            string requesturl = $"https://atlas.microsoft.com/search/nearby/{format}" +
                $"?api-version=1.0" +
                $"&lat={latitude}" +
                $"&lon={longitude}" +
                $"&subscription-key={azureMapsConfiguration.Key}";
            var result = await httpClient.GetFromJsonAsync<SearchPointsOfInterestResponse>(requesturl, cancellationToken);
            return result!;
        }

        /// <summary>
        /// Check https://learn.microsoft.com/en-us/rest/api/maps/search/get-search-poi-category?view=rest-maps-1.0&tabs=HTTP
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
