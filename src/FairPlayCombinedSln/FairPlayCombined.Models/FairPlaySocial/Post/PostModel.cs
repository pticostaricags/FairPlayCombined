using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlaySocial.Post
{
    public class PostModel : IListModel
    {
        public long PostId { get; set; }
        public short PostVisibilityId { get; set; }
        public long? PhotoId { get; set; }
        public byte PostTypeId { get; set; }
        public long? ReplyToPostId { get; set; }
        public long? GroupId { get; set; }
        public string? Text { get; set; }
        public string? OwnerApplicationUserId { get; set; }
        public string? OwnerApplicationUserName { get; set; }
    }
}
