namespace FairPlayCombined.Models.OpenAI;

public class GenerateDallE3ResponseModel
{
#pragma warning disable IDE1006 // Naming Styles
    public int created { get; set; }
    public GenerateDallE3ResponseDatumModel[]? data { get; set; }
    public long OpenAIPromptId { get; set; }
#pragma warning restore IDE1006 // Naming Styles
}

public class GenerateDallE3ResponseError
{
    public Error? error { get; set; }
}

public class Error
{
    public string? code { get; set; }
    public string? message { get; set; }
    public object? param { get; set; }
    public string? type { get; set; }
}

