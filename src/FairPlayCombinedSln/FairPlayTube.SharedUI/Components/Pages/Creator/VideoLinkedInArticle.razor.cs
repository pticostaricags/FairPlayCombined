using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class VideoLinkedInArticle
    {
        [Parameter]
        public long? VideoInfoId { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IOpenAIService? OpenAIService { get; set; }
        [Inject] IVideoCaptionsService? VideoCaptionsService { get; set; }
        [Inject] IPromptGeneratorService? PromptGeneratorService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IStringLocalizer<VideoLinkedInArticle>? Localizer { get; set; }
        private VideoInfoModel? VideoInfoModel { get; set; }
        private bool IsBusy { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private string? LinkedInArticleDraftText { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            this.VideoInfoModel = await this.VideoInfoService!.GetVideoInfoByIdAsync(this.VideoInfoId!.Value, this.cancellationTokenSource.Token);
            await this.CreateLinkedInArticleAsync();
        }

        private async Task CreateLinkedInArticleAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                var videoInfoModel = await this.VideoInfoService!.GetVideoInfoByIdAsync(id: this.VideoInfoId!.Value,
                cancellationToken: this.cancellationTokenSource.Token);
                var englishCaptions = await this.VideoCaptionsService!
                .GetVideoCaptionsByVideoInfoIdAndLanguageAsync(videoInfoId: videoInfoModel.VideoInfoId,
                language: "en-US", cancellationToken: this.cancellationTokenSource.Token);
                var promptInfo = await this.PromptGeneratorService!.GetPromptCompleteInfoAsync(promptName:
                    Constants.PromptsNames.CreateVideoLinkedInArticle, cancellationToken: this.cancellationTokenSource.Token);
                var userMessage = $"Video Title: {videoInfoModel.Description}. Video Captions: {englishCaptions}";
                var result = await this.OpenAIService!.GenerateChatCompletionAsync(promptInfo!.BaseText!, userMessage, this.cancellationTokenSource.Token);
                this.LinkedInArticleDraftText = result!.choices![0]!.message!.content!;
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

        public ValueTask DisposeAsync()
        {
            this.cancellationTokenSource.Dispose();
            return ValueTask.CompletedTask;
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Video Article")]
        public const string VideoArticleTextKey = "VideoArticleText";
        [ResourceKey(defaultValue: "Re-Create Article")]
        public const string ReCreateArticleTextKey = "ReCreateArticleText";
        #endregion Resource Keys
    }
}