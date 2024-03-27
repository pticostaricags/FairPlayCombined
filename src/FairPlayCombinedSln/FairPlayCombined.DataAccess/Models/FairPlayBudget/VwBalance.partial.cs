using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema
{
    public partial class VwBalance
    {
        [StringLength(10)]
        public string? TransactionType { get; set; }
    }
}
