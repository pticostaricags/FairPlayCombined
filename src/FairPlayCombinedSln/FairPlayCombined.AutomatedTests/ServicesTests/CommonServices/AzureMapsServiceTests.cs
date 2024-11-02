using FairPlayCombined.Models.AzureMaps;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class AzureMapsServiceTests: ServicesBase
    {
        [TestMethod]
        public async Task Test_GetNearbyAirportsAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            var endpoint = configuration["AzureMaps:Endpoint"] ??
                throw new Exception("'AzureMaps:Endpoint' is not in configuration");
            var key = configuration["AzureMaps:Key"] ??
                throw new Exception("'AzureMaps:Key' is not in configuration");
            AzureMapsConfiguration azureMapsConfiguration = new()
            {
                Endpoint = endpoint,
                Key = key,
            };
            AzureMapsService azureMapsService = new(azureMapsConfiguration,
                new HttpClient());
            double latitude = 40.6387345; 
            double longitude = -73.9529416;
            var result = await azureMapsService.GetNearbyAirportsAsync(latitude, longitude,
                CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.results!.Length > 0);
        }
    }
}
