using FairPlayCombined.Models.FairPlaySocial.Post;
using System.ComponentModel.DataAnnotations;

namespace FairPlayCombined.Models.FairPlaySocial.Notification
{
    /// <summary>
    /// Rpresents a Notification used in the SignalR communication
    /// </summary>
    public class PostNotificationModel
    {
        [Required]
        public PostAction? PostAction { get; set; }
        public string? From { get; set; }
        /// <summary>
        /// Message of the SignalR notification
        /// </summary>
        public string? Message { get; set; }
        public string? GroupName { get; set; }
        public PostModel? Post { get; set; }
    }

    public enum PostAction
    {
        PostCreated,
        PostTextUpdate,
        LikeAdded,
        LikeRemoved,
        DislikeAdded,
        DislikeRemoved,
    }
}