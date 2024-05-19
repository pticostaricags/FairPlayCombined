using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.OpenAI
{
#pragma warning disable IDE1006 // Naming Styles
    public class AnalyzeImageResponseModel
    {
        public string? id { get; set; }
        public string? _object { get; set; }
        public int created { get; set; }
        public string? model { get; set; }
        public Choice[]? choices { get; set; }
        public Usage? usage { get; set; }
        public string? system_fingerprint { get; set; }
    }

    public class Usage
    {
        public int prompt_tokens { get; set; }
        public int completion_tokens { get; set; }
        public int total_tokens { get; set; }
    }

    public class Choice
    {
        public int index { get; set; }
        public AnalyzeImageResponseMessageModel? message { get; set; }
        public object? logprobs { get; set; }
        public string? finish_reason { get; set; }
    }

    public class AnalyzeImageResponseMessageModel
    {
        public string? role { get; set; }
        public string? content { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
