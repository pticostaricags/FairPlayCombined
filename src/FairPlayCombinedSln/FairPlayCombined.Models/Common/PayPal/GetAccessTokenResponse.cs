using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.PayPal
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class GetAccessTokenResponse
    {
        public string? scope { get; set; }
        public string? access_token { get; set; }
        public string? token_type { get; set; }
        public string? app_id { get; set; }
        public int expires_in { get; set; }
        public string? nonce { get; set; }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

}
