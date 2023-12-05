using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayBudget.Currency
{
    public class CurrencyModel : IListModel
    {
        public int CurrencyId { get; set; }
        public string? Description { get; set; }
    }
}
