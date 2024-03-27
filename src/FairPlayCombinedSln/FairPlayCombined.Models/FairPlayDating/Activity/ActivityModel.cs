using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayDating
{
    public partial class ActivityModel : IListModel
    {
        public int ActivityId { get; set; }
        public string? Name { get; set; }
    }
}
