using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.OpenAI
{
#pragma warning disable IDE1006 // Naming Styles
    public class AnalyzeImageRequestModel
    {
        public string? model { get; set; }
        public Message[]? messages { get; set; }
        public int max_tokens { get; set; }
    }

    public class Message
    {
        public string? role { get; set; }
        public Content[]? content { get; set; }
    }

    public class Content
    {
        public string? type { get; set; }
        public string? text { get; set; }
        public Image_Url? image_url { get; set; }
    }

#pragma warning disable S101 // Types should be named in PascalCase
    public class Image_Url
#pragma warning restore S101 // Types should be named in PascalCase
    {
        public string? url { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
