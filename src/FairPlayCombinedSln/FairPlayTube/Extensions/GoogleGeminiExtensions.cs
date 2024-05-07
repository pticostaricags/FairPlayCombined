﻿using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.GoogleGemini;
using FairPlayCombined.Services.Common;
using Microsoft.EntityFrameworkCore;

namespace FairPlayTube.Extensions
{
    public static class GoogleGeminiExtensions
    {
        public static void AddGoogleGemini(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<GoogleGeminiConfiguration>(sp =>
            {
                IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory =
                sp.GetRequiredService<IDbContextFactory<FairPlayCombinedDbContext>>();
                var dbContext = dbContextFactory.CreateDbContext();
                var googleGeminiKeyEntity = dbContext.ConfigurationSecret.SingleOrDefault(p => p.Name == Constants.ConfigurationSecretsKeys.GOOGLE_GEMINI_KEY_KEY) ?? throw new InvalidOperationException($"Unable to find {nameof(ConfigurationSecret)} = {Constants.ConfigurationSecretsKeys.GOOGLE_GEMINI_KEY_KEY} in database");
                return new GoogleGeminiConfiguration()
                {
                    Key = googleGeminiKeyEntity.Value
                };
            });

            builder.Services.AddTransient<GoogleGeminiService>(sp =>
            {
                GoogleGeminiConfiguration googleGeminiConfiguration = sp.GetRequiredService<GoogleGeminiConfiguration>();
                HttpClient httpClient = new()
                {
                    Timeout = TimeSpan.FromMinutes(3)
                };
                return new GoogleGeminiService(googleGeminiConfiguration, httpClient);
            });
        }
    }
}