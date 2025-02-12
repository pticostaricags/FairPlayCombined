using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.InstagramApi
{

    public class UserInfoModel
    {
        public string? id { get; set; }
        public string? user_id { get; set; }
        public string? username { get; set; }
        public string? name { get; set; }
        public string? account_type { get; set; }
        public string? profile_picture_url { get; set; }
        public int followers_count { get; set; }
        public int follows_count { get; set; }
        public int media_count { get; set; }
    }

}
