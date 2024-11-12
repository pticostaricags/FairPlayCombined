using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlaySocial.VideoCaptions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Text;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class VideoCaptions
    {
        [Parameter]
        public long? VideoInfoId { get; set; }
        [Parameter]
        public string? YouTubeVideoId { get; set; }
        [Inject] IVideoCaptionsService? VideoCaptionsService { get; set; }
        [Inject] IYouTubeClientService? YouTubeClientService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IStringLocalizer<VideoCaptions>? Localizer { get; set; }
        private VideoCaptionsModel[]? Items { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        protected override async Task OnInitializedAsync()
        {
            this.Items = await this.VideoCaptionsService!
            .GetVideoCaptionsByVideoInfoIdAsync(this.VideoInfoId!.Value,
                this.cancellationTokenSource.Token
            );
        }

        private async Task OnUploadToYouTubeButtonClickedAsync(VideoCaptionsModel videoCaptionsModel)
        {
            var youTubeService = await this.YouTubeClientService!.AuthorizeAsync();
            var contentBytes = Encoding.UTF8.GetBytes(videoCaptionsModel.Content!);
            MemoryStream memoryStream = new(contentBytes);
            await this.YouTubeClientService!.UploadCaptionsAsync(this.YouTubeVideoId!,
            language: videoCaptionsModel.Language!,
            youTubeService,
            memoryStream,
            progressChanged: (progress) =>
            {
                string message = $"Status: {progress.Status}";
                if (progress.Status == Google.Apis.Upload.UploadStatus.Failed)
                {
                    message += $" - Exception: {progress.Exception.Message}";
                }
                this.ToastService!.ShowInfo(message);
            },
            responseReceived: (response) =>
            {
            },
            this.cancellationTokenSource.Token
            );
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Video Captions")]
        public const string VideoCaptionsTextKey = "VideoCaptionsText";
        [ResourceKey(defaultValue: "View On YouTube")]
        public const string ViewOnYouTubeTextKey = "ViewOnYouTubeText";
        [ResourceKey(defaultValue: "Upload To YouTube")]
        public const string UploadToYouTubeTextKey = "UploadToYouTubeText";
        [ResourceKey(defaultValue: "Language")]
        public const string LanguageTextKey = "LanguageText";
        #endregion Resource Keys
    }
}