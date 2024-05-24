using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlayTube.Conversation
{
    /// <summary>
    /// Holds the data regarding an ApplicationUser
    /// </summary>
    public class ConversationsUserModel
    {
        [Required]
        [StringLength(450)]
        /// <summary>
        /// Id of an application user
        /// </summary>
        public string? ApplicationUserId { get; set; }
        /// <summary>
        /// User's Full Name
        /// </summary>
        [Required]
        [StringLength(150)]
        public string? FullName { get; set; }
    }
}
