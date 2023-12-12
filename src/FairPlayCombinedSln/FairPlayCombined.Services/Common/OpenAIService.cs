using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common;
public class OpenAIService(HttpClient httpClient, OpenAIServiceConfiguration openAIServiceConfiguration)
{
    public async Task<ChatCompletionResponseModel?> GenerateChatCompletionAsync(
        string systemMessage, string prompt, CancellationToken cancellationToken)
    {
        var requestUrl = openAIServiceConfiguration.ChatCompletionsUrl;
        ChatCompletionRequestModel request = new ChatCompletionRequestModel()
        {
            model= "gpt-4-1106-preview",
            messages =new ChatCompletionRequestMessageModel[]
            {
                new ChatCompletionRequestMessageModel()
                {
                    content=systemMessage,
                    role="system"
                },
                new ChatCompletionRequestMessageModel()
                {
                    content=prompt,
                    role="user"
                }
            }
        };
        var response = await httpClient.PostAsJsonAsync<ChatCompletionRequestModel>(requestUrl,
            request, cancellationToken:cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ChatCompletionResponseModel>();
        return result;
    }
    public async Task<GenerateDallE3ResponseModel?> GenerateDallE3ImageAsync(string prompt, CancellationToken cancellationToken)
    {
        var requestUrl = openAIServiceConfiguration.GenerateDall3ImageUrl;
        GenerateDallE3RequestModel requestModel = new()
        {
            model = "dall-e-3",
            prompt = prompt,
            n = 1,
            size = "1024x1024"
        };
        var response = await httpClient.PostAsJsonAsync<GenerateDallE3RequestModel>(requestUrl,
            requestModel, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GenerateDallE3ResponseModel>();
        return result;

    }
}
