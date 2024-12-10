using Blazored.TextEditor;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.FairPlayBlogs.Enums;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.Common.Photo;
using FairPlayCombined.Models.FairPlayBlogs.BlogPost;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
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
        private IBlogPostService? BlogPostService { get; set; }
        [Inject]
        private IPhotoService? PhotoService { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private IStringLocalizer<CreateBlogPost>? Localizer { get; set; }
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        private EditForm? EditForm { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool IsBusy { get; set; }
        private BlazoredTextEditor? QuillHtml;

        protected override void OnInitialized()
        {
            this.CreateBlogPostModel.BlogId = BlogId;
            this.CreateBlogPostModel.BlogPostStatusId = (int)BlogPostStatus.Draft;
        }

        private async Task OnImageSelectedAsync(InputFileChangeEventArgs inputFileChangeEventArgs)
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                //Max Allowed Size = 5 megabytes
                var maxAllowedSize = 5242880;
                var stream = inputFileChangeEventArgs.File.OpenReadStream(maxAllowedSize: maxAllowedSize, this.cancellationTokenSource.Token);
                MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream);
                CreatePhotoModel createPhotoModel = new()
                {
                    Filename = inputFileChangeEventArgs.File.Name,
                    Name = inputFileChangeEventArgs.File.Name,
                    PhotoBytes = memoryStream.ToArray()
                };
                var photoId = await PhotoService!.CreatePhotoAsync(createPhotoModel, this.cancellationTokenSource.Token);
                this.CreateBlogPostModel.ThumbnailPhotoId = photoId;
            }
            catch (Exception ex)
            {
                this.ToastService?.ShowError(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
                StateHasChanged();
            }
        }

        private async Task OnSubmitAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                this.CreateBlogPostModel.Content = await this.QuillHtml!.GetHTML();
                bool isValidModel = this.EditForm!.EditContext!.Validate();
                if (isValidModel)
                {
                    await this.BlogPostService!
                    .CreateBlogPostAsync(this.CreateBlogPostModel,
                    this.cancellationTokenSource.Token);
                    this.NavigationManager!.NavigateTo("/");
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

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Create Post")]
        public const string CreatePostTextKey = "CreatePostText";
        [ResourceKey(defaultValue: "Save")]
        public const string SaveTextKey = "SaveText";
        [ResourceKey(defaultValue: "Select and image")]
        public const string SelectAnImageTextKey = "SelectAnImageText";
        [ResourceKey(defaultValue: "Title")]
        public const string TitleTextKey = "TitleText";
        [ResourceKey(defaultValue: "Preview Text")]
        public const string PreviewTextTextKey = "PreviewTextText";
        [ResourceKey(defaultValue: "Content")]
        public const string ContentTextKey = "ContentText";
        #endregion Resource Keys
    }
}
