using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayBudget.Balance
{
    public class MyBalanceModel
    {
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string? TransactionType { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public string? Description { get; set; }
        public string? MonthlyBudgetDescription { get; set; }
    }
}
