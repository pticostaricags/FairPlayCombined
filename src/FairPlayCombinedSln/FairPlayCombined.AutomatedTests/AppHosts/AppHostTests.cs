using Aspire.Hosting.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.AppHosts
{
    [TestClass]
    public class AppHostTests
    {
        [TestMethod]
        public async Task Test_RunFairPlayTubeAsync()
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.FairPlayCombinedSln_AppHost>();
            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient("fairplaytube");
            var response = await httpClient.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Test_RunFairPlatAdminPortalAsync()
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.FairPlayCombinedSln_AppHost>();
            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient("fairplayadminportal");
            var response = await httpClient.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
