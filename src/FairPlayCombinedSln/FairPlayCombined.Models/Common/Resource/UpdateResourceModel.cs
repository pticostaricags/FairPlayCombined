using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.Common.Resource
{
    public class UpdateResourceModel : IUpdateModel
    {
        [Key]
        public int ResourceId { get; set; }

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
