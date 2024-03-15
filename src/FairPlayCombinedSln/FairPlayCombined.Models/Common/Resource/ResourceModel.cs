using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Resource
{
    public class ResourceModel : IListModel
    {
        public int ResourceId { get; set; }

        public string? Type { get; set; }

        public string? Key { get; set; }

        public string? Value { get; set; }

        public int CultureId { get; set; }
        public string? CultureName { get; set; }
    }
}
