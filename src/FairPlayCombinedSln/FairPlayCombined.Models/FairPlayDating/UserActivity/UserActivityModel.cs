using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayDating.UserActivity
{
    public class UserActivityModel : IListModel
    {
        public long? UserActivityId { get; set; }
        public string? ApplicationUserId { get; set; }
        public int? ActivityId { get; set; }
        public int? FrequencyId { get; set; }
    }
}
