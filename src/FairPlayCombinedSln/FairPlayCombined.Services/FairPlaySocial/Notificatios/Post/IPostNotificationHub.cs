using FairPlayCombined.Models.FairPlaySocial.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlaySocial.Notificatios.Post
{
    public interface IPostNotificationHub
    {
        Task ReceiveMessage(PostNotificationModel model);
    }
}
