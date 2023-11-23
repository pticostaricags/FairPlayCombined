using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Common.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlaySocial.Post
{
    public class CreatePostModel : ICreateModel
    {
        [DeniedValues(default(short))]
        public short PostVisibilityId { get; set; }
        [DeniedValues(default(long))]
        public long? PhotoId { get; set; }
        [DeniedValues(default(byte))]
        public byte PostTypeId { get; set; }
        [DeniedValues(default(long))]
        public long? ReplyToPostId { get; set; }
        [DeniedValues(default(long))]
        public long? GroupId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Text { get; set; }

        [Required]
        [StringLength(450)]
        public string? OwnerApplicationUserId { get; set; }
    }
}
