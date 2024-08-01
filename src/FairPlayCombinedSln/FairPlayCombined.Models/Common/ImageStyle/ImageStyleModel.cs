using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.ImageStyle
{
    public class ImageStyleModel : IListModel
    {
        public int ImageStyleId { get; set; }
        public string? StyleName { get; set; }
    }
}
