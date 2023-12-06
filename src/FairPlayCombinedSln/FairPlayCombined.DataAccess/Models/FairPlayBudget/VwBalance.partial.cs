using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.DataAccess.Models.FairPlayBudgetSchema
{
    public partial class VwBalance
    {
        [StringLength(10)]
        public string? TransactionType { get; set; } 
    }
}
