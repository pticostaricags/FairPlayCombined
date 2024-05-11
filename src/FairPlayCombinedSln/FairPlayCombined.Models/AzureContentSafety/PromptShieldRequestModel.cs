using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.AzureContentSafety
{
#pragma warning disable IDE1006 // Naming Styles
    public class PromptShieldRequestModel
    {
        public string? userPrompt { get; set; }
        public string[]? documents { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
