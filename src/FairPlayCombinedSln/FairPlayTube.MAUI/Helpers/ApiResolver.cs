using FairPlayCombined.Interfaces.FairPlayTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayTube.MAUI.Helpers
{
    public class ApiResolver(string apiBaseUrl) : IApiResolver
    {
        public string GetBaseUrl()
        {
            return apiBaseUrl;
        }
    }
}
