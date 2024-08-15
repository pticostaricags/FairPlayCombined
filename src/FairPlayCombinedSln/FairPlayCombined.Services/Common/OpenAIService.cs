using FairPlayCombined.Common.CustomExceptions;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.OpenAI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace FairPlayCombined.Services.Common;
public class OpenAIService(
    HttpClient openAIAuthorizedHttpClient,
    HttpClient genericHttpClient,
    OpenAIServiceConfiguration openAIServiceConfiguration,
    IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
    ILogger<OpenAIService> logger, 
    IUserProviderService userProviderService) : IOpenAIService
{
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
            model = openAIServiceConfiguration.TextGenerationModel,
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
            model = openAIServiceConfiguration.TextGenerationModel,
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
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        long? errorId = null;
        try
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
                ErrorLog errorEntity = new()
                {
                    FullException = errorMessage,
                    Message = errorMessage,
                    StackTrace=System.Environment.StackTrace
                };
                await dbContext.ErrorLog.AddAsync(errorEntity, cancellationToken);
                await dbContext.SaveChangesAsync(cancellationToken);
                errorId = errorEntity.ErrorLogId;
            }
            response.EnsureSuccessStatusCode();
        }
        catch (Exception)
        {
            throw new RuleException($"An error has occured, give the following code to the admin: {errorId}");
        }
        var result = await response.Content.ReadFromJsonAsync<ChatCompletionResponseModel>(cancellationToken: cancellationToken);
        try
        {
            var userId = userProviderService.GetCurrentUserId()!;
            var promptMarginEntity = await dbContext.OpenAipromptMargin.SingleAsync(cancellationToken: cancellationToken);
            var promptCostEntity = await dbContext.OpenAipromptCost.SingleAsync(cancellationToken: cancellationToken);
            var promptCost = promptCostEntity.CostPerPrompt +
                (promptCostEntity.CostPerPrompt * promptMarginEntity.Margin);
            var userFundsEntity = await dbContext.UserFunds.
                SingleAsync(p => p.ApplicationUserId == userId,
                cancellationToken: cancellationToken);
            userFundsEntity.AvailableFunds -= promptCost;
            OpenAiprompt openAiprompt = new()
            {
                OperationCost = promptCost,
                OriginalPrompt = prompt,
                Model = request.model,
                OwnerApplicationUserId = userId,
            };
            await dbContext.OpenAiprompt.AddAsync(openAiprompt, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
            result!.OpenAIPromptId = openAiprompt.OpenAipromptId;
        }
        catch (Exception ex2)
        {
            await dbContext.ErrorLog.AddAsync(new()
            {
                FullException = ex2.ToString(),
                Message = ex2.Message,
                StackTrace = ex2.StackTrace
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
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
            size = "1792x1024"
        };
        var response = await openAIAuthorizedHttpClient.PostAsJsonAsync<GenerateDallE3RequestModel>(requestUrl,
            requestModel, cancellationToken: cancellationToken);
        var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken: cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var fullError = await response.Content.ReadAsStringAsync(cancellationToken);
            GenerateDallE3ResponseError? generateDallE3ResponseError = 
                JsonSerializer.Deserialize<GenerateDallE3ResponseError>(fullError);
            ErrorLog errorLogEntity = new()
            {
                FullException = fullError,
                StackTrace = System.Environment.StackTrace,
                Message = generateDallE3ResponseError!.error!.message
            };
            await dbContext.ErrorLog.AddAsync(errorLogEntity,cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            throw new RuleException($"Unable to generat the image, " +
                $"call your system administrator and give the following code: " +
                $"'{errorLogEntity.ErrorLogId}'");
        }
        var result = await response.Content.ReadFromJsonAsync<GenerateDallE3ResponseModel>(cancellationToken: cancellationToken);
        try
        {
            var userId = userProviderService.GetCurrentUserId()!;
            var promptMarginEntity = await dbContext.OpenAipromptMargin.SingleAsync(cancellationToken: cancellationToken);
            var promptCostEntity = await dbContext.OpenAipromptCost.SingleAsync(cancellationToken: cancellationToken);
            var promptCost = promptCostEntity.CostPerPrompt + 
                (promptCostEntity.CostPerPrompt * promptMarginEntity.Margin);
            var userFundsEntity = await dbContext.UserFunds.
                SingleAsync(p=>p.ApplicationUserId == userId,
                cancellationToken: cancellationToken);
            userFundsEntity.AvailableFunds -= promptCost;
            OpenAiprompt openAiprompt = new()
            {
                OperationCost = promptCost,
                OriginalPrompt = prompt,
                Model = model,
                OwnerApplicationUserId = userId,
                RevisedPrompt = result!.data![0].revised_prompt,
                GeneratedImageBytes = await genericHttpClient
                .GetByteArrayAsync(requestUri: result.data[0].url, cancellationToken: cancellationToken)
            };
            await dbContext.OpenAiprompt.AddAsync(openAiprompt, cancellationToken: cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken: cancellationToken);
            result.OpenAIPromptId = openAiprompt.OpenAipromptId;
        }
        catch (Exception)
        {
            //we ignore so we do not interrupt user flow
        }
        return result;

    }
}
