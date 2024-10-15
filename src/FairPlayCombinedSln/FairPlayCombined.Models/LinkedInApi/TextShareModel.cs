using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.LinkedInApi
{

    public class TextShareModel
    {
        public string? author { get; set; }
        public string? lifecycleState { get; set; }
        public Specificcontent? specificContent { get; set; }
        public Visibility? visibility { get; set; }
    }

    public class Specificcontent
    {
        [JsonPropertyName("com.linkedin.ugc.ShareContent")]
        public ComLinkedinUgcSharecontent? comlinkedinugcShareContent { get; set; }
    }

    public class ComLinkedinUgcSharecontent
    {
        public Sharecommentary? shareCommentary { get; set; }
        public string? shareMediaCategory { get; set; }
    }

    public class Sharecommentary
    {
        public string? text { get; set; }
    }

    public class Visibility
    {
        [JsonPropertyName("com.linkedin.ugc.MemberNetworkVisibility")]
        public string? comlinkedinugcMemberNetworkVisibility { get; set; }
    }

}
