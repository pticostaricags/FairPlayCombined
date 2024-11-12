using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Common.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.VideoInfo
{
    public class CreateVideoInfoModel : ICreateModel
    {
        [CustomRequired]
        public Guid AccountId { get; set; }
        [CustomRequired]
        [CustomStringLength(50)]
        public string? VideoId { get; set; }
        [CustomRequired]
        [CustomStringLength(50)]
        public string? Location { get; set; }

        [CustomRequired]
        [CustomStringLength(50)]
        public string? Name { get; set; }
        [CustomRequired]
        [CustomStringLength(500)]
        public string? Description { get; set; }

        [CustomRequired]
        [CustomStringLength(50)]
        public string? FileName { get; set; }

        [CustomStringLength(500)]
        public string? VideoBloblUrl { get; set; }

        [CustomStringLength(500)]
        public string? IndexedVideoUrl { get; set; }

        /// <summary>
        /// Video Owner Id
        /// </summary>
        [CustomRequired]
        [CustomStringLength(450)]
        public string? ApplicationUserId { get; set; }
        public int VideoIndexStatusId { get; set; }

        public double VideoDurationInSeconds { get; set; }

        [CustomStringLength(500)]
        public string? VideoIndexSourceClass { get; set; }
        public decimal Price { get; set; }
        [CustomRequired]
        [NullableUrl]
        [CustomStringLength(500)]
        public string? ExternalVideoSourceUrl { get; set; }

        [CustomStringLength(10)]
        public string? VideoLanguageCode { get; set; }
        [CustomDeniedValues(default(short))]
        public short VideoVisibilityId { get; set; }

        [CustomStringLength(500)]
        public string? ThumbnailUrl { get; set; }
        [CustomStringLength(11)]
        public string? YouTubeVideoId { get; set; }
        public bool IsVideoGeneratedWithAi { get; set; }
    }
}
