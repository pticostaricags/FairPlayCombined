using Azure.AI.OpenAI;
using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;

namespace FairPlayCombined.Services.AI.Extensions
{
    public static class SemanticKernelExtensions
    {
        public static void AddOpenAIClient(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<OpenAIClient>(sp =>
            {
                IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory =
                sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
                var dbContext = dbContextFactory.CreateDbContext();
                var openAIKeyEntity = dbContext.ConfigurationSecret
                .SingleOrDefault(p => p.Name ==
                Constants.ConfigurationSecretsKeys.OPENAI_KEY) ??
                throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.OPENAI_KEY} in database");
                OpenAIClient openAIClient = new(openAIApiKey: openAIKeyEntity.Value);
                return openAIClient;
            });
        }

        public static void AddSemanticKernel(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<Kernel>(sp =>
            {
                OpenAIClient openAIClient = sp.GetRequiredService<OpenAIClient>();
                var kernelBuilder = Kernel.CreateBuilder()
                .AddOpenAIChatCompletion(modelId: SemanticKernelService.OPENAI_MODEL,
                    openAIClient: openAIClient);
                var kernel = kernelBuilder.Build();
                return kernel;
            });
        }
    }
}
