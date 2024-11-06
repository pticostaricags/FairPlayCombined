using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.Contact
{
    public class ContactModel : IListModel
    {
        public long ContactId { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? EmailAddress { get; set; }
        public string? LinkedInProfileUrl { get; set; }
        public string? YouTubeChannelUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? XformerlyTwitterUrl { get; set; }
        public string? BusinessPhoneNumber { get; set; }
        public string? MobilePhoneNumber { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public string? Notes { get; set; }
    }
}
