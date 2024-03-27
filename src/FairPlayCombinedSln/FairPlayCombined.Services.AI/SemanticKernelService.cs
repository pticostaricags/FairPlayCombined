using FairPlayCombined.Models.AzureOpenAI;
using FairPlayCombined.Models.OpenAI;
using Microsoft.SemanticKernel;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace FairPlayCombined.Services.AI
{
    public class SemanticKernelService(AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration,
        OpenAIServiceConfiguration openAIServiceConfiguration)
    {
        private const string OPENAI_MODEL = "gpt-4-0125-preview";

        public async Task<FunctionResult?> TranslateTextAsync(string text, string fromLanguage, string toLanguage,
            CancellationToken cancellationToken)
        {
            var builder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(deploymentName: azureOpenAIServiceConfiguration.DeploymentName!,
                endpoint: azureOpenAIServiceConfiguration.Endpoint!, apiKey: azureOpenAIServiceConfiguration.Key!)
                .AddOpenAIChatCompletion(modelId: OPENAI_MODEL,
                apiKey: openAIServiceConfiguration.Key!);
            var kernel = builder.Build();
            string promptTemplate = @$"Translate text between languages. 
From Language: {{${fromLanguage}}}. 
To Language: {{${toLanguage}}}.
Text: {{${text}}}.";
            var function = kernel.CreateFunctionFromPrompt(promptTemplate);
            var result = await kernel.InvokeAsync(function, new() 
            {
                [nameof(fromLanguage)] = fromLanguage,
                [nameof(toLanguage)] = toLanguage,
                [nameof(text)] = text
            }, cancellationToken:cancellationToken);
            return result;
        }

        public async Task<FunctionResult?> CreateVideoDailyPostsAsync(string videoDescription,
            string videoEnglishCaptions,
            CancellationToken cancellationToken)
        {
            var todaysDate = DateTimeOffset.Now;
            StringBuilder systemMessage = new();
            systemMessage.AppendLine("You will take the role of an expert in Digital Marketing. I will give you the information for one of my videos. Your job is to give me a list of 30 related posts for my LinkedIn. Your response must be in HTML 5. Do not reduce the response if is lengthy, I need the full list of 30 posts.");
            systemMessage.AppendLine("Response Format: ");
            systemMessage.AppendLine($"'{todaysDate}': Post for day {todaysDate}");
            systemMessage.AppendLine($"'{todaysDate.AddDays(1)}': Post text for day {todaysDate.AddDays(1)}");
            systemMessage.AppendLine($"'{todaysDate.AddDays(2)}': Post text for day {todaysDate.AddDays(2)}");
            systemMessage.AppendLine($"'{todaysDate.AddDays(3)}': Post text for day {todaysDate.AddDays(3)}");
            systemMessage.AppendLine($"'{todaysDate.AddDays(4)}': Post textfor day {todaysDate.AddDays(4)}");
            systemMessage.AppendLine($"Video Title: {{{{${nameof(videoDescription)}}}}}. Video Captions: {{{{${nameof(videoEnglishCaptions)}}}}}");
            var promptTemplate = systemMessage.ToString();
            var builder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(deploymentName: azureOpenAIServiceConfiguration.DeploymentName!,
                endpoint: azureOpenAIServiceConfiguration.Endpoint!, apiKey: azureOpenAIServiceConfiguration.Key!)
                .AddOpenAIChatCompletion(modelId: OPENAI_MODEL,
                apiKey: openAIServiceConfiguration.Key!);
            var kernel = builder.Build();
            var function = kernel.CreateFunctionFromPrompt(promptTemplate);
            var result = await kernel.InvokeAsync(function, new()
            {
                [nameof(videoDescription)] = videoDescription,
                [nameof(videoEnglishCaptions)] = videoEnglishCaptions
            }, cancellationToken: cancellationToken);
            return result;
        }
    }
}
