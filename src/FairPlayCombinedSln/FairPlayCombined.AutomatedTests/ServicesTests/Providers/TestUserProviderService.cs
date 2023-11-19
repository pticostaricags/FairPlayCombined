using FairPlayCombined.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.AutomatedTests.ServicesTests.Providers
{
    internal class TestUserProviderService : IUserProviderService
    {
        public string GetCurrentUserId()
        {
            return "AT User";
        }
    }
}
