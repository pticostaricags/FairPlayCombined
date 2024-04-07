using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.GoogleAuth
{
#pragma warning disable IDE1006 // Naming Styles
    public class GoogleAuthClientSecretInfo
    {
        public Installed? installed { get; set; }
    }

    public class Installed
    {
        public string? client_id { get; set; }
        public string? project_id { get; set; }
        public string? auth_uri { get; set; }
        public string? token_uri { get; set; }
        public string? auth_provider_x509_cert_url { get; set; }
        public string? client_secret { get; set; }
        public string[]? redirect_uris { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }

}
