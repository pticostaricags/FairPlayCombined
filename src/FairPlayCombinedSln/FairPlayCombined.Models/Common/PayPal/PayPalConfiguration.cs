using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.PayPal
{
    public class PayPalConfiguration
    {
        public string? Endpoint { get; set; } = "https://api.sandbox.paypal.com";//For security reasons we set it to sandbox by default
        public string? ClientId { get; set; }
        public string? Secret { get; set; }
    }
}
