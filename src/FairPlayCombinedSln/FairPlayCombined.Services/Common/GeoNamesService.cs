using FairPlayCombined.Models.Common.GeoNames;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FairPlayCombined.Services.Common
{
    public class GeoNamesService(HttpClient httpClient, ILogger<GeoNamesService> logger)
    {
        public async Task<geodata> GeoRandomLocationAsync(CancellationToken cancellationToken)
        {
            string responseString = string.Empty;
            try
            {
#pragma warning disable S1075 // URIs should not be hardcoded
                var requestUrl = "https://api.3geonames.org/?randomland=yes";
#pragma warning restore S1075 // URIs should not be hardcoded
                responseString = await httpClient.GetStringAsync(requestUrl, cancellationToken);
                if (responseString[0] != '<')
                {
                    int index = responseString.IndexOf(System.Environment.NewLine);
                    responseString = responseString.Substring(index + System.Environment.NewLine.Length);
                }
                System.Xml.Serialization.XmlSerializer xmlSerializer =
                    new XmlSerializer(typeof(geodata));
                using StringReader reader = new StringReader(responseString);
                var result = (geodata)xmlSerializer.Deserialize(reader)!;
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError("{Error}", ex.Message);
                throw;
            }
        }
    }
}
