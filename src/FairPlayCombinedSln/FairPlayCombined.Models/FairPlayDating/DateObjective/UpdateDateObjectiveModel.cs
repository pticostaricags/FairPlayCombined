using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.DateObjective
{
    public class UpdateDateObjectiveModel : IUpdateModel
    {
        [Required]
        public int? DateObjectiveId { get; set; }

        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
    }
}
