using FairPlayCombined.Models.OpenAI;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IOpenAIService
    {
        Task<AnalyzeImageResponseModel?> AnalyzeImageAsync(string[] imagesBase64Strings, string prompt, 
            CancellationToken cancellationToken);
        Task<ChatCompletionResponseModel?> GenerateChatCompletionAsync(
        string systemMessage, string prompt, CancellationToken cancellationToken);
        Task<GenerateDallE3ResponseModel?> GenerateDallE3ImageAsync(string prompt, CancellationToken cancellationToken);
    }
}
