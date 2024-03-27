using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.Photo
{
    public class CreatePhotoModel : ICreateModel
    {
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
