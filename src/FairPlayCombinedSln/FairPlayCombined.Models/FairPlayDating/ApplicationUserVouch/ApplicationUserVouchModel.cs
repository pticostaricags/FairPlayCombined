using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch
{
    public class ApplicationUserVouchModel : IListModel
    {
        public long ApplicationUserVouchId { get; set; }

        public string? FromApplicationUserId { get; set; }

        public string? ToApplicationUserId { get; set; }

        public string? Description { get; set; }
    }
}
