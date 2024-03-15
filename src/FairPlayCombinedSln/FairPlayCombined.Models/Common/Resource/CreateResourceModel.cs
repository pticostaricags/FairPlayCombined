using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Resource
{
    public class CreateResourceModel : ICreateModel
    {
        [Required]
        [StringLength(1500)]
        public string? Type { get; set; }

        [Required]
        [StringLength(50)]
        public string? Key { get; set; }

        [Required]
        public string? Value { get; set; }

        public int CultureId { get; set; }
    }
}
