﻿namespace FairPlayCombined.Models.Common.PayPal
{
#pragma warning disable IDE1006 // Naming Styles
    public class GetAccessTokenResponse
    {
        public string? scope { get; set; }
        public string? access_token { get; set; }
        public string? token_type { get; set; }
        public string? app_id { get; set; }
        public int expires_in { get; set; }
        public string? nonce { get; set; }
    }
#pragma warning restore IDE1006 // Naming Styles
}
