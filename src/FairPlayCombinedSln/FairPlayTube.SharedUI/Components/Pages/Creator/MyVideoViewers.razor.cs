using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoViewer;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class MyVideoViewers
    {
        [Parameter]
        public string? VideoId { get; set; }
        [Inject] IVideoViewerService? VideoViewerService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] IStringLocalizer<MyVideoViewers>? Localizer {  get; set; }
        private bool IsBusy { get; set; }
        private GridItemsProvider<VideoViewerModel>? ItemsProvider;
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        private readonly CancellationTokenSource cancellationTokenSource = new();

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
                var items = await VideoViewerService!
                .GetPaginatedVideoViewerInfoForUserIdAsync(paginationRequest,
                    videoId: this.VideoId!,
                    userId: this.UserProviderService!.GetCurrentUserId()!,
                this.cancellationTokenSource.Token);
                StateHasChanged();
                var result = GridItemsProviderResult.From<VideoViewerModel>(items!.Items!, items.TotalItems);
                return result;
            };
            this.IsBusy = false;
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "My Video Viewers")]
        public const string MyVideoViewersTextKey = "MyVideoViewersText";
        [ResourceKey(defaultValue: "User")]
        public const string UserTextKey = "UserText";
        [ResourceKey(defaultValue: "Anonymous")]
        public const string AnonymousTextKey = "AnonymousText";
        [ResourceKey(defaultValue: "Total Time Watched")]
        public const string TotalTimeWatchedTextKey = "TotalTimeWatchedText";
        [ResourceKey(defaultValue: "Total Sessions")]
        public const string TotalSessionsTextKey = "TotalSessionsText";
        #endregion Resource Keys
    }
}