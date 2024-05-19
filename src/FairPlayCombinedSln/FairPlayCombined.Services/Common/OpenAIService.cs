using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Models.OpenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;

namespace FairPlayCombined.Services.Common;
public class OpenAIService(
    HttpClient openAIAuthorizedHttpClient,
    HttpClient genericHttpClient,
    OpenAIServiceConfiguration openAIServiceConfiguration,
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
    ILogger<OpenAIService> logger)
{

    private const string TextGenerationModel = "gpt-4o";

    public async Task<AnalyzeImageResponseModel?> AnalyzeImageAsync(
        string[] imagesBase64Strings, string prompt, CancellationToken cancellationToken)
    {
        List<Content> contents =
        [
            new()
            {
                type = "text",
                text = prompt!,
            },
        ];
        foreach (var singleImageBase64String in imagesBase64Strings)
        {
            contents.Add(new()
            {
                type = "image_url",
                image_url = new Image_Url()
                {
                    url = singleImageBase64String
                }
            });
        }
        AnalyzeImageRequestModel analyzeImageRequestModel = new()
        {
            model = TextGenerationModel,
            max_tokens = 4000,
            messages =
            [
                new Message()
                {
                    role="user",
                    content=[.. contents]
                }
             ]
        };
        var requestUrl = openAIServiceConfiguration.ChatCompletionsUrl;
        var response = await openAIAuthorizedHttpClient
            .PostAsJsonAsync(requestUrl, analyzeImageRequestModel,
            options: new()
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            }, cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AnalyzeImageResponseModel>(cancellationToken);
        return result!;
    }
    public async Task<ChatCompletionResponseModel?> GenerateChatCompletionAsync(
        string systemMessage, string prompt, CancellationToken cancellationToken)
    {
        Guid callGuid = Guid.NewGuid();
        var stopWatch = Stopwatch.StartNew();
        logger.LogInformation("Start of method: {MethodName}. CallId: {CallGuid}",
            nameof(GenerateChatCompletionAsync), callGuid);
        var requestUrl = openAIServiceConfiguration.ChatCompletionsUrl;
        ChatCompletionRequestModel request = new()
        {
            model = TextGenerationModel,
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
            request, cancellationToken: cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ChatCompletionResponseModel>(cancellationToken: cancellationToken);
        logger.LogInformation("End of method: {MethodName}. CallId: {CallGuid}. Duration: {Duration}",
            nameof(GenerateChatCompletionAsync), callGuid, stopWatch.Elapsed);
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
        var result = await response.Content.ReadFromJsonAsync<GenerateDallE3ResponseModel>(cancellationToken: cancellationToken);
        try
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
            await dbContext.OpenAiprompt.AddAsync(new OpenAiprompt()
            {
                OriginalPrompt = prompt,
                Model = model,
                RevisedPrompt = result!.data![0].revised_prompt,
                GeneratedImageBytes = await genericHttpClient
                .GetByteArrayAsync(requestUri: result.data[0].url, cancellationToken: cancellationToken)
            },
                cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
        }
        catch (Exception)
        {
            //we ignore so we do not interrupt user flow
        }
        return result;

    }
}
