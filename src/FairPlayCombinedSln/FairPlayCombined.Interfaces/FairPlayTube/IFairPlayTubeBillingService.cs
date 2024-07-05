using FairPlayCombined.Models.FairPlayTube.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IFairPlayTubeBillingService
    {
        Task<FairPlayTubeBillingModel[]> GetBillingInfoByUserIdAsync(string userId, CancellationToken cancellationToken);
    }
}
