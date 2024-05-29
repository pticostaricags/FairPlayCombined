using Aspire.Hosting.Testing;
using FairPlayCombined.Common;
using FairPlayCombinedSln.AppHost;
using Microsoft.Playwright;
using NBomber.CSharp;
using System.Net;

namespace FairPlayCombined.AutomatedTests.LoadTests
{
    [TestClass]
    public class FairPlayTubeLoadTests
    {

        [TestMethod]
        public async Task Test_Load_HomePage_Thousand_UsersAsync()
        {
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.FairPlayCombinedSln_AppHost>();
            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            var webResource = appHost.Resources
                .Single(p => p.Name == ResourcesNames.FairPlayTube);
            var endpoint = webResource
                .Annotations
                .OfType<EndpointAnnotation>()
                .Single(p => p.Name == "http");
            string url = endpoint.AllocatedEndpoint!.UriString.TrimEnd('/');

            using var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(options:new()
            {
                Headless = true
            });

            TimeSpan duration = TimeSpan.FromSeconds(30);
            var scenario =
            NBomber.CSharp.Scenario.Create(nameof(Test_Load_HomePage_Thousand_UsersAsync), 
            async context =>
            {
                var page = await browser.NewPageAsync();
                var response = await page.GotoAsync(url);
                Assert.AreEqual(url, page.Url.TrimEnd('/'));
                await page.GetByRole(Microsoft.Playwright.AriaRole.Heading,
                    new()
                    {
                        Name = Constants.ApplicationTitles.FairPlayTube
                    }).ClickAsync();

                // Assert
                Assert.IsTrue(response!.Ok);
                return Response.Ok();
            })
                .WithLoadSimulations(
                Simulation.KeepConstant(
                    copies:100,
                    during: duration));

            var stats = NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
            Assert.AreEqual(0, stats.AllFailCount);
        }
    }
}
