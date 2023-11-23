using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces
{
    public interface IUserProviderService
    {
        string? GetCurrentUserId();
        string? GetAccessToken();
    }
}
