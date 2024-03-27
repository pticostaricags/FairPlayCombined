using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.UserActivity
{
    public class UpdateUserActivityModel : IUpdateModel
    {
        [Required]
        public long? UserActivityId { get; set; }

        [Required]
        public string? ApplicationUserId { get; set; }
        [Required]
        public int? ActivityId { get; set; }
        [Required]
        public int? FrequencyId { get; set; }
    }
}
