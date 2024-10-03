using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Company
{
    public class CompanyModel : IListModel
    {
        public string? Name { get; set; }

        public string? WebsiteUrl { get; set; }

        public string? Phone { get; set; }

        public long? PrimaryContactId { get; set; }

        public string? YouTubeChannelUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? XformerlyTwitterUrl { get; set; }
        public string? OwnerApplicationUserId { get; set; }
    }
}
