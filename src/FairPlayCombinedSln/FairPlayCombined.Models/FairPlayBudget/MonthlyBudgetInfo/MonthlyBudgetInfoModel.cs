using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayBudget.MonthlyBudgetInfo
{
    public class MonthlyBudgetInfoModel : IListModel
    {
        public long MonthlyBudgetInfoId { get; set; }
        public string? Description { get; set; }
        public string? OwnerId { get; set; }
    }
}
