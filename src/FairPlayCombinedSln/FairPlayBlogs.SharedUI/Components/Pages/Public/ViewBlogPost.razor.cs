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
        [Inject]
        private IBlogPostService? BlogPostService { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private IStringLocalizer<ViewBlogPost>? Localizer { get; set; }
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
                this.BlogPostModel = await this.BlogPostService!
                    .GetBlogPostByIdAsync(this.BlogPostId, this.cancellationTokenSource.Token);
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
