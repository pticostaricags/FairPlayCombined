#if Debug_Enable_Paid_Tests
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Models.OpenAI;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class OpenAIServiceTests: ServicesBase
    {
        private const string VideoCaptions = "Speaker #1: Is Blazer good for applications that handle millions of records\r\n\r\nSpeaker #1: of data?\r\n\r\nSpeaker #1: Yes it is, especially if you use best practices such\r\n\r\nSpeaker #1: as pagination and in the case of Entity Framework code,\r\n\r\nSpeaker #1: the disabling of the changed tracker when you are going\r\n\r\nSpeaker #1: to retrieve data that is not going to be modified.\r\n\r\nSpeaker #1: As you can see in this example I am showing\r\n\r\nSpeaker #1: a list of records from a table that has 1,000,000\r\n\r\nSpeaker #1: records.\r\n\r\nSpeaker #1: The average duration for the retrieval is around 25 milliseconds.\r\n\r\nSpeaker #1: O Yes, Racer is excellent for alications that handle millions\r\n\r\nSpeaker #1: of records of data, esecially if you use best ractices.";
        
        [TestMethod]
        public async Task Test_GenerateChatCompletionAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            var sp = services.BuildServiceProvider();
            var openAIKey = configuration["OpenAIKey"] ??
                throw new Exception("'OpenAIKey' is not in configuration");
            var openAIChatCompletionsUrl = configuration["OpenAIChatCompletionsUrl"] ??
                throw new Exception("'OpenAIChatCompletionsUrl' is not in configuration");
            HttpClient httpClient = new();
            httpClient.Timeout = TimeSpan.FromMinutes(2);
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                openAIKey);
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                ChatCompletionsUrl = openAIChatCompletionsUrl
            };
            var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            OpenAIService openAIService=new(httpClient, 
                genericHttpClient:new HttpClient(),
                openAIServiceConfiguration:openAIServiceConfiguration,
                dbContextFactory:dbContextFactory);
            var systemMessage = "You will take the role of an expert in Digital Marketing. I will give you the information for one of my videos. Your job is to give me a detailed strategy on how to use the content to grow my audience. You will give me a 1 month Digital Marketing plan to repurpose the video content into LinkedIn. I post at least once a day.";
            var userMessage = $"Video Title: Is Blazor Good For Applications That Handle Millions Of Records Of Data. Video Captions: {VideoCaptions}";
            var result = await openAIService.GenerateChatCompletionAsync(systemMessage, userMessage, CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GenerateDallE3UsingSmallPromptAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            var sp = services.BuildServiceProvider();
            var openAIKey = configuration["OpenAIKey"] ??
                throw new Exception("'OpenAIKey' is not in configuration");
            var generateDall3ImageUrl = configuration["GenerateDall3ImageUrl"] ??
                throw new Exception("'GenerateDall3ImageUrl' is not in configuration");
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                openAIKey);
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                GenerateDall3ImageUrl = generateDall3ImageUrl
            };
            var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            OpenAIService openAIService =new(httpClient, new HttpClient(), 
                openAIServiceConfiguration,
                dbContextFactory);
            var result = await openAIService.GenerateDallE3ImageAsync(prompt: "logo for a Social Network app named FairPlaySocial", cancellationToken: CancellationToken.None);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Test_GenerateDallE3BasedOnVideoInformationAsync()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddUserSecrets<ServicesBase>();
            var configuration = configurationBuilder.Build();
            ServiceCollection services = new();
            var cs = _msSqlContainer!.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs, sqlServerOptionsAction => sqlServerOptionsAction.UseNetTopologySuite());
                });
            var sp = services.BuildServiceProvider();
            var openAIKey = configuration["OpenAIKey"] ??
                throw new Exception("'OpenAIKey' is not in configuration");
            var generateDall3ImageUrl = configuration["GenerateDall3ImageUrl"] ??
                throw new Exception("'GenerateDall3ImageUrl' is not in configuration");
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                openAIKey);
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                GenerateDall3ImageUrl = generateDall3ImageUrl
            };
            var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            OpenAIService openAIService = new(httpClient, new HttpClient(), 
                openAIServiceConfiguration,
                dbContextFactory);
            var result = await openAIService.GenerateDallE3ImageAsync(prompt: $"YouTube Thumbnail for video based on the following data. Video Title: Is Blazor Good For Applications That Handle Millions Of Records Of Data. Video Captions: {VideoCaptions}", cancellationToken: CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
#endif