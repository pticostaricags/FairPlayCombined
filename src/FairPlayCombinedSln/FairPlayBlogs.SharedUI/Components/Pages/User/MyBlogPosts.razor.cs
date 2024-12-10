using FairPlayCombined.Common;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayBlogs.SharedUI.Components.Pages.User;

public partial class MyBlogPosts
{
    [Parameter]
    public long BlogId { get; set; }
    [Inject]
    private IBlogPostService? BlogPostService { get; set; }
    [Inject]
    private IUserProviderService? UserProviderService { get; set; }
    [Inject]
    private NavigationManager? NavigationManager { get; set; }
    private bool IsBusy { get; set; }
    private GridItemsProvider<BlogPostModel>? ItemsProvider { get; set; }
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private PaginationState paginationState = new()
    {
        ItemsPerPage = Constants.Pagination.PageSize
    };
    protected override void OnInitialized()
    {
        this.ItemsProvider = async req => 
        {
            this.IsBusy = true;
            StateHasChanged();
            var userId = this.UserProviderService!.GetCurrentUserId();
            PaginationRequest paginationRequest = new()
            {
                PageSize = paginationState.ItemsPerPage,
                StartIndex = req.StartIndex
            };
            var items = await BlogPostService!
            .GetPaginatedBlogPostByUserIdAsync(userId!, paginationRequest, this.cancellationTokenSource.Token);
            this.IsBusy = false;
            StateHasChanged();
            var result = GridItemsProviderResult.From<BlogPostModel>(items!.Items!, items.TotalItems);
            return result;
        };
    }

    public async ValueTask DisposeAsync()
    {
        await this.cancellationTokenSource.CancelAsync();
        this.cancellationTokenSource.Dispose();
    }
}
