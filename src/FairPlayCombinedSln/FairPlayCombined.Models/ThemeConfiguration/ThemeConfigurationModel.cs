using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
