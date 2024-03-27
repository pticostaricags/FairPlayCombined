using FairPlayCombined.Models.AzureOpenAI;
using Microsoft.SemanticKernel;

namespace FairPlayCombined.Services.AI
{
    public class SemanticKernelService(AzureOpenAIServiceConfiguration azureOpenAIServiceConfiguration)
    {
        public async Task<FunctionResult?> TranslateTextAsync(string text, string fromLanguage, string toLanguage,
            CancellationToken cancellationToken)
        {
            var builder = Kernel.CreateBuilder()
                .AddAzureOpenAIChatCompletion(deploymentName: azureOpenAIServiceConfiguration.DeploymentName!,
                endpoint: azureOpenAIServiceConfiguration.Endpoint!, apiKey: azureOpenAIServiceConfiguration.Key!);
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
    }
}
