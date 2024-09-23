using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.LinkedInApi
{


    public class UserInfoModel
    {
        public string? sub { get; set; }
        public bool email_verified { get; set; }
        public string? name { get; set; }
        public Locale? locale { get; set; }
        public string? given_name { get; set; }
        public string? family_name { get; set; }
        public string? email { get; set; }
        public string? picture { get; set; }
    }

    public class Locale
    {
        public string? country { get; set; }
        public string? language { get; set; }
    }

}
