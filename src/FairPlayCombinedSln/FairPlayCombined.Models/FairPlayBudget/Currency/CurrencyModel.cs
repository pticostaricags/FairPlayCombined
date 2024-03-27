using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayBudget.Currency
{
    public class CurrencyModel : IListModel
    {
        public int CurrencyId { get; set; }
        public string? Description { get; set; }
    }
}
