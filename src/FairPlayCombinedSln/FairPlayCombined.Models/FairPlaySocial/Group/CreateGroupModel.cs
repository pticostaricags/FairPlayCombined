using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlaySocial.Group
{
    public partial class CreateGroupModel : ICreateModel
    {
        [Required]
        [StringLength(450)]
        public string? OwnerApplicationUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }

        [Required]
        [StringLength(250)]
        public string? Description { get; set; }

        [Required]
        [StringLength(100)]
        public string? TopicTag { get; set; }
    }
}
