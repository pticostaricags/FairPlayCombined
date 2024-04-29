using FairPlayCombined.Models.FairPlaySocial.Notification;
using Microsoft.AspNetCore.SignalR;

namespace FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage
{
    public class UserMessageNotificationHub : Hub<IUserMessageNotificationHub>
    {
        public async Task SendMessage(UserMessageNotificationModel model)
        {
            await Clients.Group(model.GroupName!).ReceiveMessage(model);
        }

        public Task SendMessageToCaller(UserMessageNotificationModel model)
        {
            return Clients.Caller.ReceiveMessage(model);
        }

        public Task JoinGroup(string groupName)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public Task LeaveGroup(string groupName)
        {
            return Groups
                .RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }
    }
}
