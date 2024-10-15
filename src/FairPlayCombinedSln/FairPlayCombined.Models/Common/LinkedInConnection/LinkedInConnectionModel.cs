using FairPlayCombined.Common.GeneratorsAttributes;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.Common.LinkedInConnection
{
    public class LinkedInConnectionModel: IListModel
    {
        public long LinkedInConnectionId { get; set; }

        [Required]
        [StringLength(450)]
        public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(1000)]
        [Url]
        public string? ProfileUrl { get; set; }

        [StringLength(50)]
        public string? EmailAddress { get; set; }
        [EmailAddress]

        [StringLength(1000)]
        public string? Company { get; set; }

        [StringLength(1000)]
        public string? Position { get; set; }

        public DateOnly ConnectedOn { get; set; }
    }
}
