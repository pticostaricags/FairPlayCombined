using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.Common.VisitorTracking
{
    /// <summary>
    /// Holds the information required to track visitors information/behavior
    /// </summary>
    public class VisitorTrackingModel
    {
        /// <summary>
        /// Id for the VisitorTracking record
        /// </summary>
        [DeniedValues(default(long))]
        public long VisitorTrackingId { get; set; }
        /// <summary>
        /// Visisted Url
        /// </summary>
        [Url]
        [Required]
        public string? VisitedUrl { get; set; }
        /// <summary>
        /// Logged In user Id
        /// </summary>
        [Required]
        public string? ApplicationUserId { get; set; }
        /// <summary>
        /// Id for the User's Session
        /// </summary>
        [Required]
        public Guid? SessionId { get; set; }
        /// <summary>
        /// Time in page
        /// </summary>
        [Required]
        public TimeSpan? TimeOnPage { get; set; }
    }
}
