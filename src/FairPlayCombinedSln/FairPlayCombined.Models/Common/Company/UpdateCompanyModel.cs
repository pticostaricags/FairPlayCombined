using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Company
{
    public class UpdateCompanyModel: IUpdateModel
    {

        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Url]
        public string? WebsiteUrl { get; set; }
        [Phone]
        [StringLength(50)]
        public string? Phone { get; set; }
        [DeniedValues(default(long))]
        public long? PrimaryContactId { get; set; }
        [Url]
        public string? YouTubeChannelUrl { get; set; }
        [Url]
        public string? InstagramUrl { get; set; }
        [Url]
        public string? LinkedInUrl { get; set; }
        [Url]
        public string? XformerlyTwitterUrl { get; set; }
    }
}
