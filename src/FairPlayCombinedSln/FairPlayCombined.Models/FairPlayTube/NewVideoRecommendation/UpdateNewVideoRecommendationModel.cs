using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation
{
    public class UpdateNewVideoRecommendationModel : IUpdateModel
    {
        [DeniedValues(default(long))]
        public long NewVideoRecommendationId { get; set; }

        [Required]
        [StringLength(450)]
        public string? ApplicationUserId { get; set; }

        [Required]
        public string? HtmlNewVideoRecommendation { get; set; }
    }
}
