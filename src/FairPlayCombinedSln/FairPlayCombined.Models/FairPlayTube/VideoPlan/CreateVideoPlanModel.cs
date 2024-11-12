using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;

namespace FairPlayCombined.Models.FairPlayTube.VideoPlan
{
    public class CreateVideoPlanModel : ICreateModel
    {
        [CustomRequired]
        public string? ApplicationUserId { get; set; }

        [CustomRequired]
        [CustomStringLength(50)]
        public string? VideoName { get; set; }

        [CustomRequired]
        [CustomStringLength(500)]
        public string? VideoDescription { get; set; }

        [CustomRequired]
        [CustomStringLength(3000)]
        public string? VideoScript { get; set; }
    }
}
