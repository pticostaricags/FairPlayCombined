using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo
{
    public class CreateMonthlyBudgetInfoModel : ICreateModel
    {
        [Required]
        [StringLength(150)]
        public string? Description { get; set; }
        [Required]
        public string? OwnerId { get; set; }
        [Required]
        [ValidateComplexType]
        public List<CreateTransactionModel>? Transactions { get; set; }
    }

    public class CreateTransactionModel
    {
        [Required]
        public DateTimeOffset? TransactionDateTime { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        public decimal? Amount { get; set; }
        [Required]
        public TransactionType? TransactionType { get; set; }
        [Required]
        [DeniedValues(default(long))]
        public int? CurrencyId { get; set; }
    }

    public enum TransactionType
    {
        Debit = 0,
        Credit = 1
    }
}
