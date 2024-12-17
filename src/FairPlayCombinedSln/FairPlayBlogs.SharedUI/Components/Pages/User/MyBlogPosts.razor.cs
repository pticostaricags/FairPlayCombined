using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayBlogs.SharedUI.Components.Pages.User;

public partial class MyBlogPosts
{
    [Parameter]
    public long BlogId { get; set; }
    [Inject]
    private IBlogPostService? BlogPostService { get; set; }
    [Inject]
    private NavigationManager? NavigationManager { get; set; }
    [Inject]
    private IStringLocalizer<MyBlogPosts>? Localizer { get; set; }
    private bool IsBusy { get; set; }
    private GridItemsProvider<BlogPostModel>? ItemsProvider { get; set; }
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly PaginationState paginationState = new()
    {
        ItemsPerPage = Constants.Pagination.PageSize
    };
    protected override void OnInitialized()
    {
        this.ItemsProvider = async req => 
        {
            this.IsBusy = true;
            StateHasChanged();
            PaginationRequest paginationRequest = new()
            {
                PageSize = paginationState.ItemsPerPage,
                StartIndex = req.StartIndex
            };
            var items = await BlogPostService!
            .GetPaginatedBlogPostByBlogIdAsync(this.BlogId, paginationRequest, this.cancellationTokenSource.Token);
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

    #region Resource Keys
    [ResourceKey(defaultValue: "My Blog Posts")]
    public const string MyBlogPostsTextKey = "MyBlogPostsText";
    [ResourceKey(defaultValue: "View Post")]
    public const string ViewPostTextKey = "ViewPostText";
    #endregion Resource Keys
}
