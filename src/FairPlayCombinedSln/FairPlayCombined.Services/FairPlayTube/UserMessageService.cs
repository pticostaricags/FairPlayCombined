using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.dboSchema;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.Common.UserMessage;
using FairPlayCombined.Models.FairPlayTube.Conversation;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.UserMessage;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Services.FairPlayTube
{
    public partial class UserMessageService(
        IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
        IUserProviderService userProviderService,
        IHubContext<UserMessageNotificationHub, IUserMessageNotificationHub> hubContext) : BaseService
    {
        public async Task<ConversationsUserModel[]?> GetMyConversationsUsersAsync(
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var currentUser = await
                             dbContext.AspNetUsers
                             .SingleAsync(p => p.Id ==
                             userProviderService.GetCurrentUserId(), cancellationToken: cancellationToken);
            var receivedMessagesUsers = await dbContext.UserMessage
                .Include(p => p.FromApplicationUser)
                .Where(p => p.ToApplicationUserId == currentUser.Id)
                .Select(p => p.FromApplicationUser)
                .Distinct().ToListAsync(cancellationToken);
            var sentMessagesUsers = await dbContext.UserMessage
                .Include(p => p.ToApplicationUser)
                .Where(p => p.FromApplicationUserId == currentUser.Id)
                .Select(p => p.ToApplicationUser)
                .Distinct().ToListAsync(cancellationToken);
            var result = receivedMessagesUsers
                .Union(sentMessagesUsers)
                .Distinct()
                .Select(p=>new ConversationsUserModel() 
                {
                    ApplicationUserId = p.Id,
                    FullName = p.UserName
                })
                .ToArray();
            if (result.Length == 0 )
            {
                result = await dbContext.AspNetUsers.Where(p => p.Id != currentUser.Id)
                    .Select(p=> new ConversationsUserModel()
                    {
                        ApplicationUserId = p.Id,
                        FullName = p.UserName
                    }).ToArrayAsync(cancellationToken);
            }
            return result;
        }

        public async Task<UserMessageModel[]?> GetMyConversationsWithUserAsync(string? userId, CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var currentUser = await
                dbContext.AspNetUsers
                .SingleAsync(p => p.Id ==
                userProviderService.GetCurrentUserId(), cancellationToken: cancellationToken);
            return await dbContext.UserMessage
                .Include(p => p.ToApplicationUser)
                .Include(p => p.FromApplicationUser)
                .Where(p =>
                (p.FromApplicationUserId == currentUser.Id &&
                p.ToApplicationUserId == userId)
                ||
                (p.FromApplicationUserId == userId &&
                p.ToApplicationUserId == currentUser.Id)
                )
                .OrderByDescending(p => p.RowCreationDateTime)
                .Select(p=>new UserMessageModel()
                {
                    FromApplicationUserFullName = p.FromApplicationUser.UserName,
                    Message = p.Message,
                    ReadByDestinatary = p.ReadByDestinatary,
                    RowCreationDateTime = p.RowCreationDateTime,
                    ToApplicationUserFullName = p.ToApplicationUser.UserName,
                    ToApplicationUserId = p.ToApplicationUserId
                }).ToArrayAsync(cancellationToken);
        }

        public async Task SendMessageAsync(UserMessageModel? userMessageModel,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            await dbContext.UserMessage.AddAsync(
                new UserMessage()
                {
                    FromApplicationUserId = userProviderService.GetCurrentUserId(),
                    ToApplicationUserId = userMessageModel!.ToApplicationUserId,
                    Message = userMessageModel.Message,
                }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            await hubContext.Clients.User(userMessageModel.ToApplicationUserId!)
                .ReceiveMessage(new Models.FairPlaySocial.Notification.UserMessageNotificationModel()
                {
                    Message = "You have a new message. Check your inbox to see it"
                });
        }
    }
}
