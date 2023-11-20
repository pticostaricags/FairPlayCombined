using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.TattooStatus
{
    public class UpdateTattooStatusModel : IUpdateModel
    {
        [Required]
        public short? TattooStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
    }
}
