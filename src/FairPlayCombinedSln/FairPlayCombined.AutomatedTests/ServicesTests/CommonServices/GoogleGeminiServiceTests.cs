#if Debug_Enable_Paid_Tests
using FairPlayCombined.Models.GoogleGemini;
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
    public class GoogleGeminiServiceTests: ServicesBase
    {
        [TestMethod]
        public async Task Test_GenerateContentAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<GoogleGeminiServiceTests>();
            var configuration = configurationBuilder.Build();
            var key = configuration["GoogleGemini:Key"];
            GoogleGeminiConfiguration googleGeminiConfiguration = new()
            {
                Key = key
            };
            GoogleGeminiService googleGeminiService =
                new(googleGeminiConfiguration, new HttpClient());
            string prompt = "Give me a fully detailed script, including dialogue for a 30-minute video. Video Title: \"Complete tutorial on .NET Aspire\". Video Description: \"You will learn everything about .NET Aspire\". Use the official documentation in the following links to futher improve your response:\n* https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview\n* https://learn.microsoft.com/en-us/dotnet/aspire/get-started/build-your-first-aspire-app?tabs=visual-studio\n* https://learn.microsoft.com/en-us/dotnet/aspire/get-started/add-aspire-existing-app\n* https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=visual-studio%2Cwindows\n* https://learn.microsoft.com/en-us/dotnet/aspire/whats-new/preview-4\n* https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/app-host-overview\n* https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/dashboard\n* https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/components-overview?tabs=dotnet-cli\n\nYour response must be in standard VTT format, including the full dialogue for a 30-minute video.";
            var requestModel = GenerateContentRequestModel.DefaultGenerateContentRequestModel;
            requestModel.generationConfig!.maxOutputTokens = 100000;
            requestModel.contents =
            [
                new()
                {
                    role="user",
                    parts=[
                       new()
                       {
                           text=prompt
                       }
                    ]
                }
            ];
            var result = await googleGeminiService.GenerateContentAsync(requestModel, CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
#endif