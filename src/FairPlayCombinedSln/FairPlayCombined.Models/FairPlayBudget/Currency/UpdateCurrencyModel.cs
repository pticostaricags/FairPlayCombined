using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

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
