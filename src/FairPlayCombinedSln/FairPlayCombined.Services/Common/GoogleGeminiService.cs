using FairPlayCombined.Models.GoogleGemini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public class GoogleGeminiService(GoogleGeminiConfiguration googleGeminiConfiguration,
        HttpClient httpClient)
    {
        public async Task<GenerateContentResponseModel?> GenerateContentAsync(GenerateContentRequestModel generateContentRequestModel, 
            CancellationToken cancellationToken)
        {
            string version = "v1beta"; //or v1beta
            string model = "gemini-1.0-pro-latest";
            string requestUrl = $"https://generativelanguage.googleapis.com/" +
                $"{version}/models/{model}:generateContent?key={googleGeminiConfiguration.Key}";
            var response = await httpClient.PostAsJsonAsync<GenerateContentRequestModel>(
                requestUrl, generateContentRequestModel, cancellationToken: cancellationToken);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<GenerateContentResponseModel>(cancellationToken:cancellationToken);
            return result;
        }
    }
}
