using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayBlogs;
using FairPlayCombined.Models.Common.Photo;
using FairPlayCombined.Models.FairPlayBlogs.Blog;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayBlogs.SharedUI.Components.Pages.User
{
    public partial class CreateBlog
    {
        [SupplyParameterFromForm]
        public CreateBlogModel createBlogModel { get; set; } = new();
        [Inject]
        private IUserProviderService? UserProviderService { get; set; }
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private IBlogService? BlogService { get; set; }
        [Inject]
        private IStringLocalizer<CreateBlog>? Localizer { get; set; }
        [Inject]
        private IPhotoService? PhotoService { get; set; }
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        private bool IsBusy { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();

        protected override void OnInitialized()
        {
            this.createBlogModel.OwnerApplicationUserId = 
                this.UserProviderService!.GetCurrentUserId();
        }

        private async Task OnValidSubmitAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                await this.BlogService!.CreateBlogAsync(this.createBlogModel, this.cancellationTokenSource.Token);
                this.ToastService!.ShowSuccess(Localizer![BlogCreatedTextKey]);
                this.NavigationManager!.NavigateTo("/");
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
                this.createBlogModel.HeaderPhotoId = photoId;
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

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue:"Create Blog")]
        public const string CreateBlogTextKey = "CreateBlogText";
        [ResourceKey(defaultValue: "Name")]
        public const string NameTextKey = "NameText";
        [ResourceKey(defaultValue: "Description")]
        public const string DescriptionTextKey = "DescriptionText";
        [ResourceKey(defaultValue: "Blog has been created")]
        public const string BlogCreatedTextKey = "BlogCreatedText";
        #endregion Resource Keys
    }
}
