using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.VideoInfo
{
    public class CreateVideoInfoModel: ICreateModel
    {
        [DeniedValues(default(long))]
        public long VideoInfoId { get; set; }
        [Required]
        public Guid AccountId { get; set; }
        [Required]
        [StringLength(50)]
        public string? VideoId { get; set; }
        [Required]
        [StringLength(50)]
        public string? Location { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [StringLength(50)]
        public string? FileName { get; set; }

        [StringLength(500)]
        public string? VideoBloblUrl { get; set; }

        [StringLength(500)]
        public string? IndexedVideoUrl { get; set; }

        /// <summary>
        /// Video Owner Id
        /// </summary>
        [Required]
        [StringLength(450)]
        public string? ApplicationUserId { get; set; }
        [DeniedValues(default(short))]
        public short VideoIndexStatusId { get; set; }

        public double VideoDurationInSeconds { get; set; }

        [StringLength(500)]
        public string? VideoIndexSourceClass { get; set; }
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? ExternalVideoSourceUrl { get; set; }

        [StringLength(10)]
        public string? VideoLanguageCode { get; set; }
        [DeniedValues(default(short))]
        public short VideoVisibilityId { get; set; }

        [StringLength(500)]
        public string? ThumbnailUrl { get; set; }
    }
}
