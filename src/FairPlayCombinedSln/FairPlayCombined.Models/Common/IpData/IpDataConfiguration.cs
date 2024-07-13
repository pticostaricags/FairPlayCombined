using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.IpData
{
    public class IpDataConfiguration
    {
        [Required]
        public string? Key { get; set; }
    }
}
