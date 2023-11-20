using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Photo
{
    public class PhotoModel : IListModel
    {
        public long PhotoId { get; set; }
        public string? Name { get; set; }
        public string? Filename { get; set; }
        public byte[]? PhotoBytes { get; set; }
    }
}
