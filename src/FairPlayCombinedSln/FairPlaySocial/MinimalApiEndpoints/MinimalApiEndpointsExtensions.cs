using FairPlayCombined.Common;
using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.Common.Photo;
using FairPlayCombined.Models.FairPlaySocial.Post;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.Common;
using FairPlayCombined.Services.FairPlaySocial;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;

namespace FairPlaySocial.MinimalApiEndpoints
{
    public static class MinimalApiEndpointsExtensions
    {
        public static WebApplication MapClientAppsEndpoints(this WebApplication app,
            string clientAppsAuthPolicy)
        {
            app.MapGet("/api/authtest", () =>
            {
                return "Auth worked!!!";
            }).RequireAuthorization(policyNames: clientAppsAuthPolicy);
            app.MapGet("api/photoimage/{photoId}", async (
                [FromServices] IDbContextFactory<FairPlayCombinedDbContext> dbContextFactory,
                CancellationToken cancellationToken,
                long photoId) =>
            {
                var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
                var photoEntity = await dbContext.Photo.AsNoTracking().SingleAsync(p => p.PhotoId == photoId);
                var mimeType = MediaTypeNames.Image.Png;
                return Results.File(photoEntity.PhotoBytes, contentType: mimeType);
            });
            var photosGroup = app.MapGroup("/api/photos");
            photosGroup.MapPost("/createPhoto", async (
                [FromServices] PhotoService photoService,
                CreatePhotoModel createPhotoModel,
                CancellationToken cancellationToken) =>
            {
                Validator.ValidateObject(createPhotoModel,
                        new ValidationContext(createPhotoModel), validateAllProperties: true);
                var photoId = await photoService.CreatePhotoAsync(createPhotoModel, cancellationToken);
                return photoId;
            }).ProducesValidationProblem()
            .RequireAuthorization(policyNames: clientAppsAuthPolicy);
            var postsGroup = app.MapGroup("/api/posts");
            postsGroup.MapGet("GetPaginatedPosts", async (
                [FromServices] PostService postService,
                [FromQuery] int startIndex,
                CancellationToken cancellationToken) =>
            {
                PaginationRequest paginationRequest = new PaginationRequest()
                {
                    StartIndex = startIndex,
                    PageSize = Constants.Pagination.PageSize
                };
                var result = await postService.GetPaginatedPostWithCustomProjectionAsync(paginationRequest,
                    p => new PostModel()
                    {
                        PostId = p.PostId,
                        PostVisibilityId = p.PostVisibilityId,
                        PhotoId = p.PhotoId,
                        PostTypeId = p.PostTypeId,
                        ReplyToPostId = p.ReplyToPostId,
                        GroupId = p.GroupId,
                        Text = p.Text,
                        OwnerApplicationUserId = p.OwnerApplicationUserId,
                        OwnerApplicationUserName = p.OwnerApplicationUser.UserName
                    }, cancellationToken);
                return result;
            });
            postsGroup.MapPost("createPost", async ([FromServices] PostService postService,
                [FromServices] IUserProviderService userProviderService,
                CreatePostModel createPostModel,
                CancellationToken cancellationToken) =>
                {
                    createPostModel.OwnerApplicationUserId = userProviderService.GetCurrentUserId();
                    Validator.ValidateObject(createPostModel,
                        new ValidationContext(createPostModel), validateAllProperties: true);
                    var postId = await postService.CreatePostAsync(createPostModel,
                        cancellationToken: cancellationToken);
                    await postService!.SendPostCreatedNotificationAsync(postId,
                        cancellationToken);
                    return postId;
                })
                .ProducesValidationProblem()
                .RequireAuthorization(policyNames: clientAppsAuthPolicy);
            return app;
        }
    }
}
