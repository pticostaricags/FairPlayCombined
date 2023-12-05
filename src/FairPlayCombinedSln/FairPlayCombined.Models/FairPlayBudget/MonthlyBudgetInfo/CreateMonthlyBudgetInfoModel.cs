using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [DeniedValues(default(int))]
        public int? CurrencyId { get; set; }
    }

    public enum TransactionType
    {
        Debit = 0,
        Credit = 1
    }
}
