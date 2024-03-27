using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.ConfigurationSecret
{
    public class ConfigurationSecretModel : IListModel
    {
        public int ConfigurationSecretId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        [StringLength(100)]
        public string? Value { get; set; }
    }
}
