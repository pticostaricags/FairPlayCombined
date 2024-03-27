using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayBudget.Currency
{
    public class CreateCurrencyModel : ICreateModel
    {
        [Required]
        [StringLength(50)]
        public string? Description { get; set; }
    }
}
