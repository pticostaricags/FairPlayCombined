using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlaySocialSchema;
using FairPlayCombined.Models.FairPlaySocial.Notification;
using FairPlayCombined.Models.FairPlaySocial.Post;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlaySocial.Notificatios.Post;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace FairPlayCombined.Services.FairPlaySocial
{
    [ServiceOfT<
        CreatePostModel,
        UpdatePostModel,
        PostModel,
        FairPlayCombinedDbContext,
        Post,
        PaginationRequest,
        PaginationOfT<PostModel>
        >]
    public partial class PostService : BaseService
    {
        private readonly IHubContext<PostNotificationHub, IPostNotificationHub> hubContext;

        public PostService(
            IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
            IHubContext<PostNotificationHub, IPostNotificationHub> hubContext,
            ILogger<PostService> logger):
            this(dbContextFactory, logger)
        {
            this.hubContext = hubContext;
        }

        public async Task SendPostCreatedNotificationAsync(long postId,
            CancellationToken cancellationToken)
        {
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            var postEntity = await dbContext.Post.SingleAsync(p=>p.PostId == postId, cancellationToken);
            var userEntity = await dbContext
                .AspNetUsers.SingleAsync(p => p.Id == postEntity.OwnerApplicationUserId);
            await hubContext.Clients.All.ReceiveMessage(new PostNotificationModel()
            {
                PostAction = PostAction.PostCreated,
                From = userEntity.UserName,
                GroupName = null,
                Message = postEntity.Text,
                Post = new PostModel()
                {
                    GroupId=postEntity.GroupId,
                    OwnerApplicationUserId = postEntity.OwnerApplicationUserId,
                    OwnerApplicationUserName = userEntity.UserName,
                    PhotoId = postEntity.PhotoId,
                    PostId = postEntity.PostId,
                    PostTypeId = postEntity.PostTypeId,
                    PostVisibilityId = postEntity.PostVisibilityId,
                    ReplyToPostId = postEntity.ReplyToPostId,
                    Text = postEntity.Text
                }
            });
        }
        public async Task<PaginationOfT<PostModel>> GetPaginatedPostWithCustomProjectionAsync(
    PaginationRequest paginationRequest,
    Expression<Func<Post, PostModel>> customProjection,
    CancellationToken cancellationToken
    )
        {
            PaginationOfT<PostModel> result = new();
            var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
            string orderByString = string.Empty;
            if (paginationRequest.SortingItems?.Length > 0)
                orderByString =
                    String.Join(",",
                    paginationRequest.SortingItems.Select(p => $"{p.PropertyName} {GetSortTypeString(p.SortType)}"));
            var query = dbContext.Post
                .Select(customProjection);
            if (!String.IsNullOrEmpty(orderByString))
                query = query.OrderBy(orderByString);
            result.TotalItems = await query.CountAsync(cancellationToken);
            result.PageSize = paginationRequest.PageSize;
            result.TotalPages = (int)Math.Ceiling((decimal)result.TotalItems / result.PageSize);
            result.Items = await query
                .OrderByDescending(p=>p.PostId)
            .Skip(paginationRequest.StartIndex)
            .Take(paginationRequest.PageSize)
            .ToArrayAsync(cancellationToken);
            return result;
        }
    }
}
