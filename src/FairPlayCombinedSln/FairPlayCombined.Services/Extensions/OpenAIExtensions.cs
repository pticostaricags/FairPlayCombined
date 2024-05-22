using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.OpenAI;
using FairPlayCombined.Services.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FairPlayCombined.Services.Extensions
{
    public static class OpenAIExtensions
    {
        public static void AddOpenAI(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<OpenAIService>(sp =>
            {
                IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory =
                sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
                var dbContext = dbContextFactory.CreateDbContext();
                var openAIKeyEntity = dbContext.ConfigurationSecret
                .SingleOrDefault(p => p.Name == 
                Constants.ConfigurationSecretsKeys.OPENAI_KEY) ?? 
                throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.OPENAI_KEY} in database");
                
                var generateDall3ImageUrlEntity = dbContext.ConfigurationSecret
                .SingleOrDefault(p => p.Name == 
                Constants.ConfigurationSecretsKeys.GENERATE_DALL3_IMAGE_URL_KEY) 
                ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.GENERATE_DALL3_IMAGE_URL_KEY} in database");
                
                var openAIChatCompletionEntity = dbContext.ConfigurationSecret
                .SingleOrDefault(p => p.Name == 
                Constants.ConfigurationSecretsKeys.OPENAI_CHAT_COMPLETION_URL_KEY) 
                ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.OPENAI_CHAT_COMPLETION_URL_KEY} in database");
                
                var openAITextGenerationModelEntity = dbContext.ConfigurationSecret
                .SingleOrDefault(p=>p.Name == 
                Constants.ConfigurationSecretsKeys.OPENAI_TEXT_GENERATION_MODEL_KEY)
                ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.OPENAI_TEXT_GENERATION_MODEL_KEY} in database");

                var timeoutMinutes = 3;
                HttpClient openAIAuthorizedHttpClient = new()
                {
                    Timeout = TimeSpan.FromMinutes(timeoutMinutes)
                };
                openAIAuthorizedHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    "Bearer", openAIKeyEntity.Value);
                HttpClient genericHttpClient = new()
                {
                    Timeout = TimeSpan.FromMinutes(timeoutMinutes)
                };
                var logger = sp.GetRequiredService<ILogger<OpenAIService>>();
                return new OpenAIService(openAIAuthorizedHttpClient,
                    genericHttpClient: genericHttpClient, new OpenAIServiceConfiguration()
                    {
                        Key = openAIKeyEntity.Value,
                        GenerateDall3ImageUrl = generateDall3ImageUrlEntity.Value,
                        ChatCompletionsUrl = openAIChatCompletionEntity.Value,
                        TextGenerationModel = openAITextGenerationModelEntity.Value
                    },
                dbContextFactory: dbContextFactory, logger);
            });
        }
    }
}
