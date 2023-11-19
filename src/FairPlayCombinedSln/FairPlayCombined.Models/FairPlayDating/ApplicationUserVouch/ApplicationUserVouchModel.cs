using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.ApplicationUserVouch
{
    public class ApplicationUserVouchModel: IListModel
    {
        public long ApplicationUserVouchId { get; set; }

        public string? FromApplicationUserId { get; set; }

        public string? ToApplicationUserId { get; set; }

        public string? Description { get; set; }
    }
}
