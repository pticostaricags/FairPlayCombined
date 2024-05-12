using FairPlayCombined.Models.Common.GeoNames;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Services.Common
{
    public class GeoNamesService(HttpClient httpClient, ILogger<GeoNamesService> logger)
    {
        public async Task<geodata?> GetGeoLocationDataAsync(
            double latitude,
            double longitude,
            CancellationToken cancellationToken)
        {
            try
            {
#pragma warning disable S1075 // URIs should not be hardcoded
                var requestUrl = $"https://api.3geonames.org/{latitude},{longitude}";
#pragma warning restore S1075 // URIs should not be hardcoded
                string responseString = await httpClient.GetStringAsync(requestUrl, cancellationToken);
                if (responseString[0] != '<')
                {
                    int index = responseString.IndexOf(System.Environment.NewLine);
                    responseString = responseString[(index + System.Environment.NewLine.Length)..];
                }
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                    new(typeof(geodata));
                using StringReader reader = new(responseString);
                var result = (geodata)xmlSerializer.Deserialize(reader)!;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, "{Message}", ex.Message);
            }
            return null;
        }
        public async Task<geodata?> GeoRandomLocationAsync(CancellationToken cancellationToken)
        {
            try
            {
#pragma warning disable S1075 // URIs should not be hardcoded
                var requestUrl = "https://api.3geonames.org/?randomland=yes";
#pragma warning restore S1075 // URIs should not be hardcoded
                string responseString = await httpClient.GetStringAsync(requestUrl, cancellationToken);
                if (responseString[0] != '<')
                {
                    int index = responseString.IndexOf(System.Environment.NewLine);
                    responseString = responseString[(index + System.Environment.NewLine.Length)..];
                }
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                    new(typeof(geodata));
                using StringReader reader = new(responseString);
                var result = (geodata)xmlSerializer.Deserialize(reader)!;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(exception: ex, "{Message}", ex.Message);
            }
            return null;
        }
    }
}
