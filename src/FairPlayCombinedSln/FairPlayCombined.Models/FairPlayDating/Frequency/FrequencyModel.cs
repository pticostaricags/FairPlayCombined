using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayDating.Frequency
{
    public class FrequencyModel : IListModel
    {
        public short? FrequencyId { get; set; }
        public string? Name { get; set; }
    }
}
