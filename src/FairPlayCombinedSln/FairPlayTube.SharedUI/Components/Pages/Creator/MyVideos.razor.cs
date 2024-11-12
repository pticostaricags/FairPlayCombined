using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class MyVideos
    {
        [Inject] IStringLocalizer<MyVideos>? Localizer { get; set; }
        [Inject] IJSRuntime? JsRuntime { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IOpenAIService? OpenAIService { get; set; }
        [Inject] IVideoCaptionsService? VideoCaptionsService { get; set; }
        [Inject] IVideoDigitalMarketingDailyPostsService? VideoDigitalMarketingDailyPostsService { get; set; }
        [Inject] IDialogService? DialogService { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        private bool IsBusy { get; set; }
        private GridItemsProvider<VideoInfoModel>? ItemsProvider;
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private static CultureInfo CurrencyCulture = new(Constants.Cultures.CurrencyDefaultCulture);
        private const string CurrencyFormat = Constants.Cultures.CurrencyDefaultFormat;

        protected override void OnInitialized()
        {
            ItemsProvider = async req =>
            {
                try
                {
                    this.IsBusy = true;
                    StateHasChanged();
                    PaginationRequest paginationRequest = new()
                    {
                        PageSize = paginationState.ItemsPerPage,
                        StartIndex = req.StartIndex,
                        SortingItems = [new SortingItem()
                    {
                        PropertyName=nameof(VideoInfoModel.RowCreationDateTime),
                        SortType= SortType.Descending
                    }]
                    };
                    var items = await VideoInfoService!.GetPaginatedCompletedVideoInfobyUserIdAsync(paginationRequest,
                        userId: this.UserProviderService!.GetCurrentUserId()!,
                    this.cancellationTokenSource.Token);
                    this.IsBusy = false;
                    StateHasChanged();
                    var result = GridItemsProviderResult.From<VideoInfoModel>(items!.Items!, items.TotalItems);
                    return result;
                }
                catch (Exception ex)
                {
                    this.ToastService!.ShowError(ex.Message);
                    return GridItemsProviderResult.From<VideoInfoModel>(
                        Array.Empty<VideoInfoModel>(), 0);
                }
            };
        }

        private async Task OnVideoDeletedAsync()
        {
            await this.paginationState.SetCurrentPageIndexAsync(this.paginationState.CurrentPageIndex);
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "My Videos")]
        public const string MyVideosPageTitleTextKey = "MyVideosPageTitleText";
        [ResourceKey(defaultValue: "Indexing Cost")]
        public const string IndexingCostTextKey = "IndexingCostText";
        [ResourceKey(defaultValue: "YouTube Video Id")]
        public const string YouTubeVideoIdTextKey = "YouTubeVideoIdText";
        [ResourceKey(defaultValue: "Delete Video")]
        public const string DeleteVideoTextKey = "DeleteVideoText";
        [ResourceKey(defaultValue: "Create YouTube Thumbnail")]
        public const string CreateYouTubeThumbnailTextKey = "CreateYouTubeThumbnailText";
        [ResourceKey(defaultValue: "Create Infographic")]
        public const string CreateInfographicTextKey = "CreateInfographicText";
        [ResourceKey(defaultValue: "Get LinkedIn Marketing Ideas")]
        public const string GetLinkedInMarketingIdeasTextKey = "GetLinkedInMarketingIdeasText";
        [ResourceKey(defaultValue: "Get New LinkedIn Daily Posts Ideas")]
        public const string GetNewLinkedInDailyPostsIdeasTextKey = "GetNewLinkedInDailyPostsIdeasText";
        [ResourceKey(defaultValue: "Get Passive Income Ideas")]
        public const string GetPassiveIncomeIdeasTextKey = "GetPassiveIncomeIdeasText";
        [ResourceKey(defaultValue: "Create LinkedIn Article")]
        public const string CreateLinkedInArticleTextKey = "CreateLinkedInArticleText";
        [ResourceKey(defaultValue: "View My Video Viewers")]
        public const string ViewMyVideoViewersTextKey = "ViewMyVideoViewersText";
        [ResourceKey(defaultValue: "Manage YouTube Captions")]
        public const string ManageYouTubeCaptionsTextKey = "ManageYouTubeCaptionsText";
        [ResourceKey(defaultValue: "Watch on YouTube")]
        public const string WatchOnYouTubeTextKey = "WatchOnYouTubeText";
        [ResourceKey(defaultValue: "Create YouTube Chapters")]
        public const string CreateYouTubeChaptersTextKey = "CreateYouTubeChaptersText";
        [ResourceKey(defaultValue: "Export Video Data")]
        public const string ExportVideoDataTextKey = "ExportVideoDataText";
        #endregion Resource Keys
    }
}