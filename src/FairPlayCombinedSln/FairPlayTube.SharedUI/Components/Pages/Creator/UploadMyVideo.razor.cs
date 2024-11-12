using FairPlayCombined.Common;
using FairPlayCombined.Common.FairPlayTube.Enums;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureVideoIndexer;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using Google.Apis.YouTube.v3.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Common.CustomAttributes;
using Microsoft.Extensions.Localization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class UploadMyVideo : IAsyncDisposable
    {
        [SupplyParameterFromForm]
        private CreateVideoInfoModel CreateVideoInfoModel { get; set; } = new();
        [Inject] IAzureVideoIndexerService? AzureVideoIndexerService { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] IYouTubeClientService? YouTubeClientService { get; set; }
        [Inject] ILogger<UploadMyVideo>? Logger { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        [Inject] AzureVideoIndexerServiceConfiguration? AzureVideoIndexerServiceConfiguration { get; set; }
        [Inject] IStringLocalizer<UploadMyVideo>? Localizer { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool IsBusy { get; set; }
        private EditForm? frmCreateVideoInfo;
        private long BytesSent { get; set; }
        private bool HideSearchYoutubeVideosDialog { get; set; } = false;
        private FluentDialog? SearchYoutubeVideosDialog { get; set; }
        private bool IsAuthenticatedWithGoogle { get; set; }

        protected override void OnInitialized()
        {
            this.IsAuthenticatedWithGoogle = this.UserProviderService!.IsAuthenticatedWithGoogle();
            this.CreateVideoInfoModel.AccountId = Guid.Parse(AzureVideoIndexerServiceConfiguration!.AccountId!);
            this.CreateVideoInfoModel.Location = AzureVideoIndexerServiceConfiguration!.Location;
            this.CreateVideoInfoModel.VideoVisibilityId = 1;
            this.CreateVideoInfoModel.FileName = "Test";
            this.CreateVideoInfoModel.VideoVisibilityId = (short)VideoVisibility.Public;
            this.CreateVideoInfoModel.ApplicationUserId = this.UserProviderService!.GetCurrentUserId();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                this.SearchYoutubeVideosDialog!.Hide();
            }
        }

        private async Task<bool> OnProcessFileButtonClickedAsync()
        {
            bool hasError;
            try
            {
                Logger!.LogInformation(message: "Processing video from url: {Url}", this.CreateVideoInfoModel.ExternalVideoSourceUrl);
                this.IsBusy = true;
                StateHasChanged();
                this.frmCreateVideoInfo!.EditContext!.Validate();
                var nameField = frmCreateVideoInfo!.EditContext!.Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.Name));
                var descriptionField = frmCreateVideoInfo!.EditContext!.Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.Name));
                var externalVideoSourceUrl = frmCreateVideoInfo!.EditContext!.Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.ExternalVideoSourceUrl));
                if (!frmCreateVideoInfo.EditContext.IsValid(nameField) ||
                !frmCreateVideoInfo.EditContext.IsValid(descriptionField) ||
                !frmCreateVideoInfo.EditContext.IsValid(externalVideoSourceUrl))
                {
                    this.IsBusy = false;
                    StateHasChanged();
                    this.ToastService!.ShowError(Localizer![FillAllValuesTextKey]);
#pragma warning disable S1854 // Unused assignments should be removed
                    hasError = true;
#pragma warning restore S1854 // Unused assignments should be removed
                }
                var authToken = await this.AzureVideoIndexerService!.AuthenticateToAzureArmAsync();
                var accountAccessToken = await this.AzureVideoIndexerService!.GetAccessTokenForArmAccountAsync(authToken, this.cancellationTokenSource.Token);
                var indexVideoResult = await this.AzureVideoIndexerService
                .IndexVideoFromUriAsync(
                    new IndexVideoFromUriParameters()
                    {
                        ArmAccessToken = accountAccessToken!.AccessToken!,
                        Description = this.CreateVideoInfoModel.Description!,
                        FileName = this.CreateVideoInfoModel.Name!,
                        Name = this.CreateVideoInfoModel.Name!,
                        VideoUri = new Uri(this.CreateVideoInfoModel.ExternalVideoSourceUrl!)
                    });
                this.CreateVideoInfoModel.VideoId = indexVideoResult!.id;
                this.CreateVideoInfoModel.VideoIndexStatusId = (short)VideoIndexStatus.Processing;
                this.frmCreateVideoInfo.EditContext.Validate();
                this.ToastService!.ShowInfo(Localizer![VideoSentToBeProcessedTextKey]);
                hasError = false;
            }
            catch (Exception ex)
            {
                hasError = true;
                Logger!.LogError(exception: ex, message: "Failed to process video. {Message}", ex.Message);
                this.ToastService!.ShowError(ex.Message);
            }
            this.IsBusy = false;
            StateHasChanged();
            return hasError;
        }

        public async Task UploadToYouTubeAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                this.frmCreateVideoInfo!.EditContext!.Validate();
                var nameField = frmCreateVideoInfo!.EditContext!.Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.Name));
                var descriptionField = frmCreateVideoInfo!.EditContext!.Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.Name));
                var externalVideoSourceUrl = frmCreateVideoInfo!.EditContext!.Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.ExternalVideoSourceUrl));
                if (!frmCreateVideoInfo.EditContext.IsValid(nameField) ||
                !frmCreateVideoInfo.EditContext.IsValid(descriptionField) ||
                !frmCreateVideoInfo.EditContext.IsValid(externalVideoSourceUrl))
                {
                    this.ToastService!.ShowError(Localizer![FillAllValuesTextKey]);
                    return;
                }
                var videoStream = await new HttpClient().GetStreamAsync(this.CreateVideoInfoModel.ExternalVideoSourceUrl);
                Google.Apis.YouTube.v3.Data.Video video = new()
                {
                    Snippet = new VideoSnippet()
                };
                video.Snippet.Title = this.CreateVideoInfoModel.Name;
                video.Snippet.Description = this.CreateVideoInfoModel.Description;
                video.Status = new VideoStatus
                {
                    PrivacyStatus = "unlisted"
                };
                await this.YouTubeClientService!.UploadVideoAsync(video, videoStream,
                progressChanged: async (uploadProgress) =>
                {
                    this.BytesSent = uploadProgress.BytesSent;
                    await InvokeAsync(() => StateHasChanged());
                },
                responseReceived: (video) =>
                {
                    this.CreateVideoInfoModel.YouTubeVideoId = video.Id;
                    this.ToastService!.ShowSuccess($"{Localizer![VideoUploadedToYouTubeTextKey]}. Id: {video.Id}");
                });
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            this.IsBusy = false;
            StateHasChanged();
        }

        private async Task OnValidSubmitAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                bool hasError = await OnProcessFileButtonClickedAsync();
                if (!hasError)
                {
                    this.CreateVideoInfoModel.VideoIndexStatusId = (short)VideoIndexStatus.Processing;
                    await this.VideoInfoService!.CreateVideoInfoAsync(this.CreateVideoInfoModel,
                    this.cancellationTokenSource.Token);
                    this.ToastService!.ShowSuccess(Localizer![VideoSavedTextKey]);
                    this.NavigationManager!.NavigateTo(Constants.Routes.FairPlayTubeRoutes.CreatorRoutes.MyProcessingVideos);
                }
            }
            catch (Exception ex)
            {
                Logger!.LogError(exception: ex, message: "Error: {ErrorMessage}", ex.Message);
                this.ToastService!.ShowError(ex.Message);
            }
            this.IsBusy = false;
            StateHasChanged();
        }

        private async Task PerformSubmitAsync()
        {
            await this.frmCreateVideoInfo!.OnValidSubmit.InvokeAsync();
        }

        private void OpenSearchYouTubeVideoDialog()
        {
            this.SearchYoutubeVideosDialog!.Show();
        }

        private void CloseSearchYouTubeVideoDialog()
        {
            this.SearchYoutubeVideosDialog!.Hide();
        }

        private void OnYouTubeVideoSelected(string youTubeVideoId)
        {
            this.CreateVideoInfoModel!.YouTubeVideoId = youTubeVideoId;
            CloseSearchYouTubeVideoDialog();
        }

        private void OnVideoNameChange(FluentWizardStepChangeEventArgs fluentWizardStepChangeEventArgs)
        {
            var videonameField = this.frmCreateVideoInfo!.EditContext!
            .Field(nameof(CreateVideoInfoModel.Name));
            this.frmCreateVideoInfo.EditContext.Validate();
            var isVideonameFieldValid = this.frmCreateVideoInfo.EditContext.IsValid(videonameField);
            fluentWizardStepChangeEventArgs.IsCancelled = !isVideonameFieldValid;
        }

        private void OnVideoDescriptionStepChange(FluentWizardStepChangeEventArgs fluentWizardStepChangeEventArgs)
        {
            var descriptonField = this.frmCreateVideoInfo!.EditContext!
            .Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.Description));
            this.frmCreateVideoInfo.EditContext.Validate();
            var isDescriptonFieldValid = this.frmCreateVideoInfo.EditContext.IsValid(descriptonField);
            fluentWizardStepChangeEventArgs.IsCancelled = !isDescriptonFieldValid;
        }

        private void OnVideoUrlChange(FluentWizardStepChangeEventArgs fluentWizardStepChangeEventArgs)
        {
            var externalVideoSourceUrlField =
            this.frmCreateVideoInfo!.EditContext!
            .Field(nameof(FairPlayCombined.Models.FairPlayTube.VideoInfo.CreateVideoInfoModel.ExternalVideoSourceUrl));
            this.frmCreateVideoInfo.EditContext.Validate();
            bool isExternalVideoSourceUrlFieldValid =
            this.frmCreateVideoInfo.EditContext.IsValid(externalVideoSourceUrlField);
            fluentWizardStepChangeEventArgs.IsCancelled = !isExternalVideoSourceUrlFieldValid;
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Upload My Video")]
        public const string UploadMyVideoTextKey = "UploadMyVideoText";
        [ResourceKey(defaultValue: "Name")]
        public const string NameTextKey = "Name";
        [ResourceKey(defaultValue: "Was the Video Generated Using AI")]
        public const string WasVideoGeneratedUsingAITextKey = "WasVideoGeneratedUsingAIText";
        [ResourceKey(defaultValue:"Description")]
        public const string DescriptionTextKey = "DescriptionText";
        [ResourceKey(defaultValue: "Video Url")]
        public const string VideoUrlTextKey = "VideoUrlText";
        [ResourceKey(defaultValue: "Here you can upload your video to YouTube if you want")]
        public const string YouCanUploadToYouTubeTextKey = "YouCanUploadToYouTubeText";
        [ResourceKey(defaultValue: "Upload To YouTube")]
        public const string UploadToYouTubeTextKey = "UploadToYouTubeText";
        [ResourceKey(defaultValue: "Bytes Sent")]
        public const string BytesSentTextKey = "BytesSentText";
        [ResourceKey(defaultValue: "Video Id (YouTube)")]
        public const string YouTubeVideoIdTextKey = "YouTubeVideoIdText";
        [ResourceKey(defaultValue: "Search My YouTube Videos")]
        public const string SearchMyVideosTextKey = "SearchMyVideosText";
        [ResourceKey(defaultValue: "Close")]
        public const string CloseTextKey = "CloseText";
        [ResourceKey(defaultValue: "Please make sure to fill all the values and try again")]
        public const string FillAllValuesTextKey = "FillAllValuesText";
        [ResourceKey(defaultValue: "Your video has been sent to be processed")]
        public const string VideoSentToBeProcessedTextKey = "VideoSentToBeProcessedText";
        [ResourceKey(defaultValue: "Video uploaded to YouTube")]
        public const string VideoUploadedToYouTubeTextKey = "VideoUploadedToYouTubeText";
        [ResourceKey(defaultValue: "Your video has been saved. You'll be notified when indexing is ready. It will take a while for your video to become visible")]
        public const string VideoSavedTextKey = "VideoSavedText";
        #endregion Resource Keys
    }
}