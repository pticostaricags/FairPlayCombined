using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class VideoChapters
    {
        [Parameter]
        public long? VideoInfoId { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IOpenAIService? OpenAIService { get; set; }
        [Inject] IVideoCaptionsService? VideoCaptionsService { get; set; }
        [Inject] IStringLocalizer<VideoChapters>? Localizer { get; set; }
        private VideoInfoModel? VideoInfoModel { get; set; }
        private bool IsBusy { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private string? YouTubeChapters { get; set; }

        protected override async Task OnInitializedAsync()
        {
            this.IsBusy = true;
            StateHasChanged();
            this.VideoInfoModel = await this.VideoInfoService!.GetVideoInfoByIdAsync(id: this.VideoInfoId!.Value,
            cancellationToken: this.cancellationTokenSource.Token);
            StateHasChanged();
            await this.GenerateYouTubeChaptersAsync(VideoInfoModel);
            this.IsBusy = false;
            StateHasChanged();
        }

        private async Task OnRecreatePlanButtonclickedAsync()
        {
            this.IsBusy = true;
            StateHasChanged();
            await this.GenerateYouTubeChaptersAsync(this.VideoInfoModel!);
            this.IsBusy = false;
            StateHasChanged();
        }

        private async Task GenerateYouTubeChaptersAsync(VideoInfoModel videoInfoModel)
        {
            var englishCaptions = await this.VideoCaptionsService!
                .GetVideoCaptionsByVideoInfoIdAndLanguageAsync(videoInfoId: videoInfoModel.VideoInfoId,
                language: "en-US", cancellationToken: this.cancellationTokenSource.Token);
            var systemMessage = "You will take the role of an expert in YouTube SEO. I will give you the information for one of my videos. Your job is to generate the repective YouTube Chapters, timestamp must be in format: hh:mm:ss, do not add milliseconds, always start with 00:00:00. Return them as an HTML 5 bullet list.";
            var userMessage = $"Video Title: {videoInfoModel.Description}. Video Captions: {englishCaptions}";
            var result = await this.OpenAIService!.GenerateChatCompletionAsync(systemMessage, userMessage, this.cancellationTokenSource.Token);
            if (result != null)
            {
                this.YouTubeChapters = result!.choices![0]!.message!.content!;
            }
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Video Chapters")]
        public const string VideoChaptersTextKey = "VideoChaptersText";
        [ResourceKey(defaultValue: "Re-Create Chapters")]
        public const string ReCreateChaptersTextKey = "ReCreateChaptersText";
        #endregion Resource Keys
    }
}