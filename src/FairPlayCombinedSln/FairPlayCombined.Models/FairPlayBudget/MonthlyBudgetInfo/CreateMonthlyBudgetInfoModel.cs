using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo
{
    public class CreateMonthlyBudgetInfoModel : ICreateModel
    {
        [Required]
        [StringLength(150)]
        public string? Description { get; set; }

        [Required]
        [StringLength(450)]
        public string? OwnerId { get; set; }
    }
}
