﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.VideoComment
{
    public class UpdateVideoCommentModel : IUpdateModel
    {
        [DeniedValues(default(long))]
        public long VideoCommentId { get; set; }
        [DeniedValues(default(long))]
        public long VideoInfoId { get; set; }

        [Required]
        [StringLength(450)]
        public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Comment { get; set; }
    }
}
