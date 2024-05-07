using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.Common
{
    public partial class UserFundService(PayPalOrderService payPalOrderService)
    {
        public async Task AddMyFundsAsync(string orderId, CancellationToken cancellationToken)
        {
            await payPalOrderService.GetOrderDetailsAsync(orderId, cancellationToken);
        }
    }
}
