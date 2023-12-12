namespace FairPlayCombined.Services.Common;

public class ChatCompletionResponseChoiceModel
{
    public int index { get; set; }
    public ChatCompletionResponseMessageModel? message { get; set; }
    public string? finish_reason { get; set; }
}
