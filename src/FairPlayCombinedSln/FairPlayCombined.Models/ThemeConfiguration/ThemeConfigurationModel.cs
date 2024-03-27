using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.ThemeConfiguration
{
    public class ThemeConfigurationModel
    {
        [Required]
        public string? Key { get; set; }
        [Required]
        public string? Value { get; set; }
    }
}
