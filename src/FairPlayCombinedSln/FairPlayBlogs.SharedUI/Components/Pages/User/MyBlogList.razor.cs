using FairPlayCombined.Common;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayBlogs.SharedUI.Components.Pages.User
{
    public partial class MyBlogList
    {
        [Inject]
        private IBlogService? BlogService { get; set; }
        [Inject]
        private IUserProviderService? UserProviderService { get; set; }
        private GridItemsProvider<BlogModel>? ItemsProvider { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private PaginationState paginationState = new()
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
    }
}
