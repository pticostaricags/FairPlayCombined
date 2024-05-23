namespace FairPlayCombined.Models.OpenAI;

public class ChatCompletionResponseChoiceModel
{
#pragma warning disable IDE1006 // Naming Styles
    public int index { get; set; }
    public ChatCompletionResponseMessageModel? message { get; set; }
    public string? finish_reason { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}
