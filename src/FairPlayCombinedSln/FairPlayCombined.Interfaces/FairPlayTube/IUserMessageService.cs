using FairPlayCombined.Models.Common.UserMessage;
using FairPlayCombined.Models.FairPlayTube.Conversation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IUserMessageService
    {
        Task<ConversationsUserModel[]?> GetMyConversationsUsersAsync(CancellationToken cancellationToken);
        Task<UserMessageModel[]?> GetMyConversationsWithUserAsync(string? userId, CancellationToken cancellationToken);
        Task SendMessageAsync(UserMessageModel? userMessageModel, CancellationToken cancellationToken);
    }
}
