using FairPlayCombined.Models.FairPlaySocial.Notification;

namespace FairPlayCombined.Services.FairPlaySocial.Notificatios.Post
{
    public interface IPostNotificationHub
    {
        Task ReceiveMessage(PostNotificationModel model);
    }
}
