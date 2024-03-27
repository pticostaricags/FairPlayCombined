using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo
{
    public class MonthlyBudgetInfoModel : IListModel
    {
        public long MonthlyBudgetInfoId { get; set; }
        public string? Description { get; set; }
        public string? OwnerId { get; set; }
    }
}
