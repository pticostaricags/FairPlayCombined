using FairPlayCombined.Models.Common.UserMessage;
using FairPlayCombined.Models.FairPlayTube.Conversation;

namespace FairPlayCombined.Interfaces.FairPlayTube
{
    public interface IUserMessageService
    {
        Task<ConversationsUserModel[]?> GetMyConversationsUsersAsync(CancellationToken cancellationToken);
        Task<UserMessageModel[]?> GetMyConversationsWithUserAsync(string? userId, CancellationToken cancellationToken);
        Task SendMessageAsync(UserMessageModel? userMessageModel, CancellationToken cancellationToken);
    }
}
