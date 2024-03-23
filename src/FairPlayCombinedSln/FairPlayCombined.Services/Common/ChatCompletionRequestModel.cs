namespace FairPlayCombined.Services.Common;

public class ChatCompletionRequestModel
{
#pragma warning disable IDE1006 // Naming Styles
    public string? model { get; set; }
    public ChatCompletionRequestMessageModel[]? messages { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}
