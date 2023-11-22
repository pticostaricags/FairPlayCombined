using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.AzureOpenAI
{
    public class TextModerationResponse
    {
        public string? TextModerated { get; set; }
        public bool IsSexuallyExplicit { get; set; }
        public string[]? SexuallyExplicitPhrases { get; set; }
        public bool IsSexuallySuggestive { get; set; }
        public string[]? SexuallySuggestivePhrases { get; set; }
        public bool IsOffensive { get; set; }
        public string[]? OffensivePhrases { get; set; }
        public string[]? PersonalIdentifiableInformation { get; set; }
        public bool HasPersonalIdentifiableInformation { get; set; }
        public string[]? Profanity { get; set; }
    }
}
