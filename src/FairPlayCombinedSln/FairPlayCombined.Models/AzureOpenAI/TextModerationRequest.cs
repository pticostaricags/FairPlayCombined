using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.AzureOpenAI
{
    public class TextModerationRequest
    {
        public string? TextToModerate { get; set; }
    }
}
