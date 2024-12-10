using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs.Blog;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayBlogs.SharedUI.Components.Pages.User
{
    public partial class MyBlogList
    {
        [Inject]
        private IBlogService? BlogService { get; set; }
        [Inject]
        private IUserProviderService? UserProviderService { get; set; }
        [Inject]
        private IStringLocalizer<MyBlogList>? Localizer { get; set; }
        private GridItemsProvider<BlogModel>? ItemsProvider { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        private bool IsBusy { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            ItemsProvider = async req =>
            {
                this.IsBusy = true;
                StateHasChanged();
                var userId = this.UserProviderService!.GetCurrentUserId();
                PaginationRequest paginationRequest = new()
                {
                    PageSize = paginationState.ItemsPerPage,
                    StartIndex = req.StartIndex
                };
                var items = await BlogService!
                .GetAllBlogByUserIdAsync(userId!, paginationRequest, this.cancellationTokenSource.Token);
                this.IsBusy = false;
                StateHasChanged();
                var result = GridItemsProviderResult.From<BlogModel>(items!.Items!, items.TotalItems);
                return result;
            };
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "My Blog List")]
        public const string MyBlogListTextKey = "MyBlogListText";
        [ResourceKey(defaultValue: "Actions")]
        public const string ActionsTextKey = "ActionsText";
        [ResourceKey(defaultValue: "Create Post")]
        public const string CreatePostTextKey = "CreatePostText";
        private string CreatePostText => Localizer![CreatePostTextKey];
        [ResourceKey(defaultValue: "My Blog Posts")]
        public const string MyBlogPostsTextKey = "MyBlogPostsText";
        private string MyBlogPostsText => Localizer![MyBlogPostsTextKey];
        #endregion Resource Keys
    }
}
