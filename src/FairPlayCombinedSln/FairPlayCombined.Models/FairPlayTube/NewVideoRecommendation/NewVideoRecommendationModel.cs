using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation
{
    public class NewVideoRecommendationModel : IListModel
    {
        public long NewVideoRecommendationId { get; set; }
        public string? ApplicationUserId { get; set; }
        public string? HtmlNewVideoRecommendation { get; set; }
    }
}
