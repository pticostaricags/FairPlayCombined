using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.VideoPlan
{
    public class UpdateVideoPlanModel : IUpdateModel
    {
        public long VideoPlanId { get; set; }

        [CustomRequired]
        public string? ApplicationUserId { get; set; }

        [CustomRequired]
        [CustomStringLength(50)]
        public string? VideoName { get; set; }

        [CustomRequired]
        [CustomStringLength(500)]
        public string? VideoDescription { get; set; }

        [CustomRequired]
        [CustomStringLength(1000)]
        public string? VideoScript { get; set; }
    }
}
