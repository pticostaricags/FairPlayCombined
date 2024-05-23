using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.Common
{
    public interface IUserFundService
    {
        Task<decimal> GetMyAvailableFundsAsync(CancellationToken cancellationToken);
        Task AddMyFundsAsync(string orderId, CancellationToken cancellationToken);
    }
}
