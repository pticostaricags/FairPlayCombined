using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Photo
{
    public class UpdatePhotoModel : IUpdateModel
    {
        [DeniedValues(default(long))]
        public long PhotoId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string? Filename { get; set; }

        [Required]
        public byte[]? PhotoBytes { get; set; }
    }
}
