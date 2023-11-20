using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.UserActivity
{
    public class CreateUserActivityModel : ICreateModel
    {
        [Required]
        public string? ApplicationUserId { get; set; }
        [Required]
        public int ActivityId { get; set; }
        [Required]
        public int FrequencyId { get; set; }
    }
}
