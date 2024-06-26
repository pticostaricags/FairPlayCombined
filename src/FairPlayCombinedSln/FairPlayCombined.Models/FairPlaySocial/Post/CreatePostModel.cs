﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlaySocial.Post
{
    public class CreatePostModel : ICreateModel
    {
        [DeniedValues(default(int))]
        public int PostVisibilityId { get; set; }
        [DeniedValues(default(long))]
        public long? PhotoId { get; set; }
        [DeniedValues(default(int))]
        public int PostTypeId { get; set; }
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
