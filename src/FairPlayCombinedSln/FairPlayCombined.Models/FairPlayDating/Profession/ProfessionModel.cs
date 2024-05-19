using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.Profession
{
    public class ProfessionModel: IListModel
    {
        public int ProfessionId { get; set; }

        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
    }
}
