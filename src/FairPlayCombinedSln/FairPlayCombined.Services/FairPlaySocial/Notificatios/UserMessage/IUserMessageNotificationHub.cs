using FairPlayCombined.Models.FairPlaySocial.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage
{
    public interface IUserMessageNotificationHub
    {
        Task ReceiveMessage(UserMessageNotificationModel model);
    }
}
