using Blazored.TextEditor;
using FairPlayBlogs.SharedUI.Components.Pages.User;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayBlogs.SharedUI.Components.Pages.Public
{
    public partial class ViewBlogPost
    {
        [Parameter]
        public long BlogPostId { get; set; }
        [Parameter]
        public string? BlogName { get; set; }
        [Parameter]
        public string? BlogPostTitle { get; set; }
        [Inject]
        private IBlogPostService? BlogPostService { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private IStringLocalizer<ViewBlogPost>? Localizer { get; set; }
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool IsBusy { get; set; }
        private BlogPostModel? BlogPostModel { get; set; }
        private BlazoredTextEditor? QuillHtml;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                if (this.BlogName?.Length > 0 && this.BlogPostTitle?.Length > 0)
                {
                    var decodedBlogName = System.Net.WebUtility.UrlDecode(this.BlogName);
                    var decodedBlogPostTitle = System.Net.WebUtility.UrlDecode(this.BlogPostTitle);
                    this.BlogPostModel = await this.BlogPostService!
                        .GetBlogPostByBlogNameAndPostTitleAsync(decodedBlogName, decodedBlogPostTitle, this.cancellationTokenSource.Token);
                }
                else
                {
                    this.BlogPostModel = await this.BlogPostService!
                        .GetBlogPostByIdAsync(this.BlogPostId, this.cancellationTokenSource.Token);
                }
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
                StateHasChanged();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
            {
                await this.QuillHtml!.LoadHTMLContent(this.BlogPostModel!.Content);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "View Blog Post")]
        public const string ViewBlogPostTextKey = "ViewBlogPostText";
        #endregion Resource Keys
    }
}
