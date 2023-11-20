using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.UserActivity
{
    public class UserActivityModel : IListModel
    {
        public long? UserActivityId { get; set; }
        public string? ApplicationUserId { get; set; }
        public short? ActivityId { get; set; }
        public short? FrequencyId { get; set; }
    }
}
