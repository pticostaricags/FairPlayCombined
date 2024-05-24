using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayDating.Profession
{
    public class ProfessionModel : IListModel
    {
        public int ProfessionId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
    }
}
