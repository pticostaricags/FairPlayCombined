using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.Billing
{
    public class FairPlayTubeBillingModel
    {
        public decimal OperationCost { get; set; }
        public DateTimeOffset RowCreationDateTime { get; set; }
        public string? Details { get; set; }
    }
}
