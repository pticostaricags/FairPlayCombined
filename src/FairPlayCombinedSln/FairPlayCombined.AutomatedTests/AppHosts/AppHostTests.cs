using Aspire.Hosting.Testing;
using FairPlayCombinedSln.AppHost;
using System.Net;

namespace FairPlayCombined.AutomatedTests.AppHosts
{
    [TestClass]
    public class AppHostTests
    {
        [TestMethod]
        public async Task Test_RunFairPlayTubeAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.FairPlayCombinedSln_AppHost>();
            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            var httpClient = app.CreateHttpClient(ResourcesNames.FairPlayTube);
            var response = await httpClient.GetAsync("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Test_RunFairPlayAdminPortalAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.FairPlayCombinedSln_AppHost>();
            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            var httpClient = app.CreateHttpClient(ResourcesNames.FairPlayAdminPortal);
            var response = await httpClient.GetAsync("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
