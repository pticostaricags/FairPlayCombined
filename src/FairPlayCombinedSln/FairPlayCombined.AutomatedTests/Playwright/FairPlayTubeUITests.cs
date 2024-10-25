using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Testing;
using FairPlayCombinedSln.AppHost;
using Microsoft.Playwright.MSTest;

namespace FairPlayCombined.AutomatedTests.Playwright
{
    [TestClass]
    public class FairPlayTubeUITests : PageTest
    {
        [TestMethod]
        public async Task Test_LoadHomePageAsync()
        {
            Page.SetDefaultNavigationTimeout(60000);
            var appHost = await DistributedApplicationTestingBuilder
                .CreateAsync<Projects.FairPlayCombinedSln_AppHost>();
            await using var app = await appHost.BuildAsync();
            await app.StartAsync();

            var webResource = appHost.Resources
                .Single(p => p.Name == ResourcesNames.FairPlayTube);
            var endpoint = webResource
                .Annotations
                .OfType<EndpointAnnotation>()
                .Single(p => p.Name == "https");
            await this.Page.GotoAsync(endpoint.AllocatedEndpoint!.UriString);
            await Expect(this.Page)
                .ToHaveURLAsync($"{endpoint.AllocatedEndpoint!.UriString}/");
            await this.Page.GetByRole(Microsoft.Playwright.AriaRole.Heading,
                new()
                {
                    Name = "The Next Generation Of Video Sharing Portals"
                }).ClickAsync();
        }
    }
}
