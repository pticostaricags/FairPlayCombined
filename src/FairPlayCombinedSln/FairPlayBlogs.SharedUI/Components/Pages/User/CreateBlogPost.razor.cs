using Blazored.TextEditor;
using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayBlogs.SharedUI.Components.Pages.User
{
    public partial class CreateBlogPost
    {
        [Parameter]
        public long BlogId { get; set; }
        [SupplyParameterFromForm]
        private CreateBlogPostModel CreateBlogPostModel { get; set; } = new();
        [Inject]
        private IToastService? ToastService { get; set; }
        private BlazoredTextEditor? QuillHtml;

        protected override void OnInitialized()
        {
            this.CreateBlogPostModel.BlogId = BlogId;
        }

        private async Task OnValidSubmitAsync()
        {
            var html = await this.QuillHtml!.GetHTML();
            this.ToastService!.ShowSuccess(html);
        }
    }
}
