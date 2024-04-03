using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.GoogleGemini
{

    public class GenerateContentResponseModel
    {
        public GenerateContentResponseCandidate[]? candidates { get; set; }
        public GenerateContentResponsePromptfeedback? promptFeedback { get; set; }
    }

    public class GenerateContentResponsePromptfeedback
    {
        public GenerateContentResponseSafetyrating[]? safetyRatings { get; set; }
    }

    public class GenerateContentResponseSafetyrating
    {
        public string? category { get; set; }
        public string? probability { get; set; }
    }

    public class GenerateContentResponseCandidate
    {
        public Content? content { get; set; }
        public string? finishReason { get; set; }
        public int index { get; set; }
        public GenerateContentResponseSafetyrating1[]? safetyRatings { get; set; }
    }

    public class GenerateContentResponseContent
    {
        public GenerateContentResponsePart[]? parts { get; set; }
        public string? role { get; set; }
    }

    public class GenerateContentResponsePart
    {
        public string? text { get; set; }
    }

    public class GenerateContentResponseSafetyrating1
    {
        public string? category { get; set; }
        public string? probability { get; set; }
    }

}
