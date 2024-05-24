using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.VideoPlan
{
    public class VideoPlanModel : IListModel
    {
        public long VideoPlanId { get; set; }
        [Required]
        public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? VideoName { get; set; }

        [Required]
        [StringLength(500)]
        public string? VideoDescription { get; set; }

        [Required]
        [StringLength(1000)]
        public string? VideoScript { get; set; }
    }
}
