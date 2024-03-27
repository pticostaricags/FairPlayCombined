using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.AzureOpenAI
{
    public class AzureOpenAIServiceConfiguration
    {
        public string? DeploymentName { get; set; }
        public string? Endpoint { get; set; }
        public string? Key { get; set; }
    }
}
