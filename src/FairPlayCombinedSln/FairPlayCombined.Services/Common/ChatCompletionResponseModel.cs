namespace FairPlayCombined.Services.Common;

public class ChatCompletionResponseModel
{
    public string? id { get; set; }
    public string? _object { get; set; }
    public int created { get; set; }
    public string? model { get; set; }
    public ChatCompletionResponseChoiceModel[]? choices { get; set; }
    public ChatCompletionResponseUsageModel? usage { get; set; }
    public string? system_fingerprint { get; set; }
}
