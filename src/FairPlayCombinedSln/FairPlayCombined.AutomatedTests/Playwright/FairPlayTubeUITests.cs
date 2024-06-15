using Aspire.Hosting.Testing;
using FairPlayCombined.Common;
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
            await this.Page.GotoAsync(endpoint.AllocatedEndpoint!.UriString);
            await Expect(this.Page)
                .ToHaveURLAsync($"{endpoint.AllocatedEndpoint!.UriString}/");
            await this.Page.GetByRole(Microsoft.Playwright.AriaRole.Heading,
                new()
                {
                    Name = Constants.ApplicationTitles.FairPlayTube
                }).ClickAsync();
        }
    }
}
