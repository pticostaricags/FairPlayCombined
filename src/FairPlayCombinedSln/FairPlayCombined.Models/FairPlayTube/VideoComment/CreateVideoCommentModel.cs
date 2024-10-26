using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.VideoComment
{
    public class CreateVideoCommentModel : ICreateModel
    {
        [CustomDeniedValues(default(long))]
        public long VideoInfoId { get; set; }

        [CustomRequired]
        [CustomStringLength(450)]
        public string? ApplicationUserId { get; set; }

        [CustomRequired]
        [CustomStringLength(500)]
        public string? Comment { get; set; }
    }
}
