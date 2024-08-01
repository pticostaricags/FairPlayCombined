using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable S101 // Types should be named in PascalCase
namespace FairPlayCombined.Models.Common.IpData
{
    public class GetGeoLocationInfoResponse
    {
        public string? ip { get; set; }
        public bool is_eu { get; set; }
        public string? city { get; set; }
        public string? region { get; set; }
        public string? region_code { get; set; }
        public string? country_name { get; set; }
        public string? country_code { get; set; }
        public string? continent_name { get; set; }
        public string? continent_code { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
        public string? postal { get; set; }
        public string? calling_code { get; set; }
        public string? flag { get; set; }
        public string? emoji_flag { get; set; }
        public string? emoji_unicode { get; set; }
        public Asn? asn { get; set; }
        public Language[]? languages { get; set; }
        public Currency? currency { get; set; }
        public Time_Zone? time_zone { get; set; }
        public Threat? threat { get; set; }
        public string? count { get; set; }
    }

    public class Asn
    {
        public string? asn { get; set; }
        public string? name { get; set; }
        public string? domain { get; set; }
        public string? route { get; set; }
        public string? type { get; set; }
    }

    public class Currency
    {
        public string? name { get; set; }
        public string? code { get; set; }
        public string? symbol { get; set; }
        public string? native { get; set; }
        public string? plural { get; set; }
    }

    public class Time_Zone
    {
        public string? name { get; set; }
        public string? abbr { get; set; }
        public string? offset { get; set; }
        public bool? is_dst { get; set; }
        public DateTime? current_time { get; set; }
    }

    public class Threat
    {
        public bool is_tor { get; set; }
        public bool is_proxy { get; set; }
        public bool is_anonymous { get; set; }
        public bool is_known_attacker { get; set; }
        public bool is_known_abuser { get; set; }
        public bool is_threat { get; set; }
        public bool is_bogon { get; set; }
    }

    public class Language
    {
        public string? name { get; set; }
        public string? native { get; set; }
        public string? code { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles
#pragma warning restore S101 // Types should be named in PascalCase