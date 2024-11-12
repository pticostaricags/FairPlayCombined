using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class MyProcessingVideos
    {
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] IStringLocalizer<MyProcessingVideos>? Localizer { get; set; }
        private bool IsBusy { get; set; }
        private GridItemsProvider<VideoInfoModel>? ItemsProvider;
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private readonly System.Timers.Timer timer = new()
        {
            AutoReset = false,
            Interval = TimeSpan.FromMinutes(1).TotalMilliseconds
        };

        protected override void OnInitialized()
        {
            this.IsBusy = true;
            ItemsProvider = async req =>
            {
                StateHasChanged();
                PaginationRequest paginationRequest = new()
                {
                    PageSize = paginationState.ItemsPerPage,
                    StartIndex = req.StartIndex
                };
                var items = await VideoInfoService!.GetPaginatedNotCompletedVideoInfobyUserIdAsync(paginationRequest,
                    userId: this.UserProviderService!.GetCurrentUserId()!,
                this.cancellationTokenSource.Token);
                StateHasChanged();
                var result = GridItemsProviderResult.From<VideoInfoModel>(items!.Items!, items.TotalItems);
                return result;
            };
            this.IsBusy = false;
            timer.Elapsed += async (sender, args) =>
            {
                await InvokeAsync(async () =>
                {
                    await this.paginationState.SetCurrentPageIndexAsync(this.paginationState.CurrentPageIndex);
                });
            };
            timer.Start();
        }

        public async ValueTask DisposeAsync()
        {
            timer.Stop();
            timer.Dispose();
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "My Processing Videos")]
        public const string MyProcessingVideosTextKey = "MyProcessingVideosText";
        [ResourceKey(defaultValue: "Name")]
        public const string NameTextKey = "NameText";
        [ResourceKey(defaultValue: "Video Id")]
        public const string VideoIdTextKey = "VideoIdText";
        [ResourceKey(defaultValue: "YouTube Video Id")]
        public const string YouTubeVideoIdTextKey = "YouTubeVideoIdText";
        [ResourceKey(defaultValue: "Processing %")]
        public const string ProcessingPercentageTextKey = "ProcessingPercentageText";
        #endregion Resource Keys
    }
}