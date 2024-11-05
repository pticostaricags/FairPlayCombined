#if Debug_Enable_Paid_Tests
using FairPlayCombined.AutomatedTests.ServicesTests.Providers;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Models.OpenAI;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.AutomatedTests.ServicesTests.CommonServices
{
    [TestClass]
    public class OpenAIServiceTests : ServicesBase
    {
        private const string VideoCaptions = "Speaker #1: Is Blazer good for applications that handle millions of records\r\n\r\nSpeaker #1: of data?\r\n\r\nSpeaker #1: Yes it is, especially if you use best practices such\r\n\r\nSpeaker #1: as pagination and in the case of Entity Framework code,\r\n\r\nSpeaker #1: the disabling of the changed tracker when you are going\r\n\r\nSpeaker #1: to retrieve data that is not going to be modified.\r\n\r\nSpeaker #1: As you can see in this example I am showing\r\n\r\nSpeaker #1: a list of records from a table that has 1,000,000\r\n\r\nSpeaker #1: records.\r\n\r\nSpeaker #1: The average duration for the retrieval is around 25 milliseconds.\r\n\r\nSpeaker #1: O Yes, Racer is excellent for alications that handle millions\r\n\r\nSpeaker #1: of records of data, esecially if you use best ractices.";
        private const string TEXT_GENERATION_MODEL = "gpt-4o";

        [TestMethod]
        public async Task Test_AnalyzeImageAsync()
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
            var openAIKey = configuration["OpenAI:Key"] ??
                throw new Exception("'OpenAI:Key' is not in configuration");
            var openAIChatCompletionsUrl = configuration["OpenAIChatCompletionsUrl"] ??
                throw new Exception("'OpenAIChatCompletionsUrl' is not in configuration");
            var imageToAnalyzeFilePath = configuration["ImageToAnalyzeFilePath"] ??
                throw new Exception("'ImageToAnalyzeFilePath' is not in configuration");
            HttpClient httpClient = new()
            {
                Timeout = TimeSpan.FromMinutes(2)
            };
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                openAIKey);
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                ChatCompletionsUrl = openAIChatCompletionsUrl,
                TextGenerationModel= TEXT_GENERATION_MODEL
            };
            var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<OpenAIService>();
            OpenAIService openAIService = new(httpClient,
                genericHttpClient: new HttpClient(),
                openAIServiceConfiguration: openAIServiceConfiguration,
                dbContextFactory: dbContextFactory,
                logger: logger, 
                userProviderService:new TestUserProviderService());
            string prompt = "Analyze this image";
            byte[] imageBytes = await File.ReadAllBytesAsync(imageToAnalyzeFilePath);
            string imageBase64String= $"data:image/jpg;base64, {Convert.ToBase64String(imageBytes)}";
            var result = await openAIService.AnalyzeImageAsync([imageBase64String], prompt, CancellationToken.None);
            Assert.IsNotNull(result);
        }

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
            var openAIKey = configuration["OpenAI:Key"] ??
                throw new Exception("'OpenAI:Key' is not in configuration");
            var openAIChatCompletionsUrl = configuration["OpenAIChatCompletionsUrl"] ??
                throw new Exception("'OpenAIChatCompletionsUrl' is not in configuration");
            HttpClient httpClient = new()
            {
                Timeout = TimeSpan.FromMinutes(2)
            };
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",
                openAIKey);
            OpenAIServiceConfiguration openAIServiceConfiguration = new()
            {
                ChatCompletionsUrl = openAIChatCompletionsUrl,
                TextGenerationModel = TEXT_GENERATION_MODEL
            };
            var dbContextFactory = sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<OpenAIService>();
            OpenAIService openAIService = new(httpClient,
                genericHttpClient: new HttpClient(),
                openAIServiceConfiguration: openAIServiceConfiguration,
                dbContextFactory: dbContextFactory,
                logger: logger,
                userProviderService:new TestUserProviderService());
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
            var openAIKey = configuration["OpenAI:Key"] ??
                throw new Exception("'OpenAI:Key' is not in configuration");
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
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<OpenAIService>();
            OpenAIService openAIService = new(httpClient, new HttpClient(),
                openAIServiceConfiguration,
                dbContextFactory,
                logger: logger,
                userProviderService:new TestUserProviderService());
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
            var openAIKey = configuration["OpenAI:Key"] ??
                throw new Exception("'OpenAI:Key' is not in configuration");
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
            var loggerFactory = LoggerFactory.Create(p => p.AddConsole());
            var logger = loggerFactory!.CreateLogger<OpenAIService>();
            OpenAIService openAIService = new(httpClient, new HttpClient(),
                openAIServiceConfiguration,
                dbContextFactory,
                logger:logger,
                userProviderService:new TestUserProviderService());
            var result = await openAIService.GenerateDallE3ImageAsync(prompt: $"YouTube Thumbnail for video based on the following data. Video Title: Is Blazor Good For Applications That Handle Millions Of Records Of Data. Video Captions: {VideoCaptions}", cancellationToken: CancellationToken.None);
            Assert.IsNotNull(result);
        }
    }
}
#endif