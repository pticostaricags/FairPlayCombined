using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.AzureContentSafety
{
    public class AzureContentSafetyConfiguration
    {
        public string? Endpoint { get; set; }
        public string? Key { get; set; }
        public string? ApiVersion { get; set; }
    }
}
