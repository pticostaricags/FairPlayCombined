using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common;
public class OpenAIService(
    HttpClient openAIAuthorizedHttpClient, 
    HttpClient genericHttpClient,
    OpenAIServiceConfiguration openAIServiceConfiguration,
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory)
{
    public async Task<ChatCompletionResponseModel?> GenerateChatCompletionAsync(
        string systemMessage, string prompt, CancellationToken cancellationToken)
    {
        var requestUrl = openAIServiceConfiguration.ChatCompletionsUrl;
        ChatCompletionRequestModel request = new()
        {
            model= "gpt-4-1106-preview",
            messages =
            [
                new()
                {
                    content=systemMessage,
                    role="system"
                },
                new()
                {
                    content=prompt,
                    role="user"
                }
            ]
        };
        var response = await openAIAuthorizedHttpClient.PostAsJsonAsync<ChatCompletionRequestModel>(requestUrl,
            request, cancellationToken:cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ChatCompletionResponseModel>(cancellationToken:cancellationToken);
        return result;
    }
    public async Task<GenerateDallE3ResponseModel?> GenerateDallE3ImageAsync(string prompt, CancellationToken cancellationToken)
    {
        if (prompt.Length > 4000)
            throw new RuleException($"{nameof(prompt)} is too long, please make it shorter");
        var requestUrl = openAIServiceConfiguration.GenerateDall3ImageUrl;
        string model = "dall-e-3";
        GenerateDallE3RequestModel requestModel = new()
        {
            model = model,
            prompt = prompt,
            n = 1,
            size = "1024x1024"
        };
        var response = await openAIAuthorizedHttpClient.PostAsJsonAsync<GenerateDallE3RequestModel>(requestUrl,
            requestModel, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GenerateDallE3ResponseModel>(cancellationToken:cancellationToken);
        try
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken:cancellationToken);
            await dbContext.OpenAiprompt.AddAsync(new OpenAiprompt() 
            {
                OriginalPrompt = prompt,
                Model = model,
                RevisedPrompt = result!.data![0].revised_prompt,
                GeneratedImageBytes=await genericHttpClient
                .GetByteArrayAsync(requestUri: result.data[0].url,cancellationToken:cancellationToken)
            },
                cancellationToken:cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            //we ignore so we do not interrupt user flow
        }
        return result;

    }
}
