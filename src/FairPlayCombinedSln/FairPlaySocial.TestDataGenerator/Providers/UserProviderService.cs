using FairPlayCombined.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlaySocial.TestDataGenerator.Providers
{
    public class UserProviderService : IUserProviderService
    {
        internal static string? UserId { get; set; }
        public string GetCurrentUserId()
        {
            return UserId;
        }
    }
}
