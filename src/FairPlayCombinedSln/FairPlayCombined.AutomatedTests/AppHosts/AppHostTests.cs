using Aspire.Hosting.Testing;
using FairPlayCombinedSln.AppHost;
using NBomber.CSharp;
using System.Net;

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
            var httpClient = app.CreateHttpClient(ResourcesNames.FairPlayTube);
            var scenario =
            NBomber.CSharp.Scenario.Create("load_home_page", async context =>
            {
                var response = await httpClient.GetAsync("/");

                // Assert
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                return Response.Ok();
            })
                .WithLoadSimulations(
                Simulation.Inject(
                    rate: 10,
                    interval: TimeSpan.FromSeconds(1),
                    during: TimeSpan.FromSeconds(30)));

            var stats = NBomberRunner
                .RegisterScenarios(scenario)
                .Run();

            var failedStats = stats.ScenarioStats[0].Fail;
            Assert.AreEqual(0, failedStats.Request.Count);
        }

        [TestMethod]
        public async Task Test_RunFairPlayAdminPortalAsync()
        {
            // Arrange
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.FairPlayCombinedSln_AppHost>();
            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            // Act
            var httpClient = app.CreateHttpClient(ResourcesNames.FairPlayAdminPortal);
            var response = await httpClient.GetAsync("/");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
