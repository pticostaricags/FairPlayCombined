namespace FairPlayCombined.Services.Common;

public class ChatCompletionRequestModel
{
    public string? model { get; set; }
    public ChatCompletionRequestMessageModel[]? messages { get; set; }
}
