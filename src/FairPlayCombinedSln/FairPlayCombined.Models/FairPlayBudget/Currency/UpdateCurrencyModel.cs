using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayBudget.Currency
{
    public class UpdateCurrencyModel : IUpdateModel
    {
        [DeniedValues(default(int))]
        public int CurrencyId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Description { get; set; }
    }
}
