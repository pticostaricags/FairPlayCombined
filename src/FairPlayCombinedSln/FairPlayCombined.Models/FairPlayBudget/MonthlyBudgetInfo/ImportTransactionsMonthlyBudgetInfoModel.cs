using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo
{
#pragma warning disable IDE1006 // Naming Styles
    public class ImportTransactionsMonthlyBudgetInfoModel
    {
        public string? oficina { get; set; }
        public string? fechaMovimiento { get; set; }
        public string? numeroDocumento { get; set; }
        public decimal? debito { get; set; }
        public decimal? credito { get; set; }
        public string? descripcion { get; set; }
    }
}
#pragma warning restore IDE1006 // Naming Styles