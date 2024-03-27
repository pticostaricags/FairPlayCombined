using FairPlayCombined.Models.FairPlaySocial.Notification;

namespace FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage
{
    public interface IUserMessageNotificationHub
    {
        Task ReceiveMessage(UserMessageNotificationModel model);
    }
}
