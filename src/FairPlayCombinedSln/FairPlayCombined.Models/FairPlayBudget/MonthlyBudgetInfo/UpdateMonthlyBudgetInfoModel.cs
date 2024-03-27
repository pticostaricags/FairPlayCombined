using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo
{
    public class UpdateMonthlyBudgetInfoModel : IUpdateModel
    {
        [DeniedValues(default(long))]
        public long MonthlyBudgetInfoId { get; set; }

        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        [Required]
        [StringLength(450)]
        public string? OwnerId { get; set; }
    }
}
