using FairPlayCombined.Models.AzureMaps;
using FairPlayCombined.Models.AzureMaps.SearchPOICategoryTree;
using FairPlayCombined.Services.Common;
using Microsoft.Extensions.Configuration;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class AzureMapsServiceTests : ServicesBase
    {
        [TestMethod]
        public async Task Test_GetPointsOfInterestCategoryTreeAsync()
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
            var result = await azureMapsService.GetPointsOfInterestCategoryTreeAsync(CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.poiCategories!.Length > 0);
            var convertedList = result.ToPOICategoryModelList();
            Assert.IsNotNull(convertedList);
            Assert.IsTrue(convertedList.SelectMany(p=>p.Children!).Any());
        }

        [TestMethod]
        public async Task Test_GetNearbyPointsOfInterestAsync()
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
            var result = await azureMapsService.GetNearbyPointsOfInterestAsync(latitude, longitude,
                CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.IsTrue(result.results!.Length > 0);
        }

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
