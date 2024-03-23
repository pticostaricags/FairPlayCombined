namespace FairPlayCombined.Services.Common;

public class ChatCompletionResponseUsageModel
{
#pragma warning disable IDE1006 // Naming Styles
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}
