using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.ImageStyle
{
    public class UpdateImageStyleModel: IUpdateModel
    {
        [DeniedValues(0)]
        public int ImageStyleId { get; set; }

        [Required]
        [StringLength(50)]
        public string? StyleName { get; set; }
    }
}
