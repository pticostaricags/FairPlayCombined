using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch
{
    public class UpdateApplicationUserVouchModel: IUpdateModel
    {
        [Required]
        [DeniedValues(0)]
        public long? ApplicationUserVouchId { get; set; }

        [Required]
        public string? FromApplicationUserId { get; set; }

        [Required]
        public string? ToApplicationUserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Description { get; set; }
    }
}
