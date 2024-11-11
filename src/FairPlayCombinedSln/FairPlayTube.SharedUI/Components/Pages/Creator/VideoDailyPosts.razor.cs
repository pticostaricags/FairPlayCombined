using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoDigitalMarketingDailyPosts;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Globalization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class VideoDailyPosts
    {
        [Parameter]
        public long? VideoInfoId { get; set; }
        [Inject] IVideoDigitalMarketingDailyPostsService? VideoDigitalMarketingDailyPostsService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] ICustomCache? CustomCache { get; set; }
        [Inject] ICultureService? CultureService { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IStringLocalizer<VideoDailyPosts>? Localizer { get; set; }
        private VideoInfoModel? VideoInfoModel { get; set; }
        private bool IsBusy { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private string? DigitalMarketingDailyPostsIdeas { get; set; }
        private GridItemsProvider<VideoDigitalMarketingDailyPostsModel>? ItemsProvider { get; set; }
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };

        private CultureInfo[]? SupportedCultures { get; set; }
        private CultureInfo? SelectedCulture { get; set; }
        private string? SelectedSocialNetwork { get; set; } = "LinkedIn";

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        protected async Task LoadDataAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                this.VideoInfoModel = await this.VideoInfoService!.GetVideoInfoByIdAsync(this.VideoInfoId!.Value, this.cancellationTokenSource.Token);
                var items = await this.CustomCache!.GetOrCreateAsync<string[]>(
                    key: nameof(this.SupportedCultures),
                    retrieveDataTask: async () =>
                    {
                        var data = (await this.CultureService!
                            .GetSupportedCultures(this.cancellationTokenSource.Token));
                        return data;
                    },
                    expiration: Constants.CacheConfiguration.LocalizationCacheDuration,
                    this.cancellationTokenSource.Token);
                this.SupportedCultures ??= [.. items
                .Select(p => new CultureInfo(p))
                .OrderBy(p => p.DisplayName)];
                this.SelectedCulture ??= CultureInfo.CurrentCulture;
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
            ItemsProvider = async req =>
                {
                    this.IsBusy = true;
                    StateHasChanged();
                    PaginationRequest paginationRequest = new()
                    {
                        PageSize = Constants.Pagination.PageSize,
                        StartIndex = req.StartIndex,
                        SortingItems =
                        [
                        new SortingItem()
                        {
                            PropertyName=nameof(VideoDigitalMarketingDailyPostsModel.VideoDigitalMarketingDailyPostsId),
                            SortType= SortType.Descending
                        }
                        ]
                    };
                    var items = await this.VideoDigitalMarketingDailyPostsService!
                    .GetPaginatedVideoDigitalMarketingDailyPostsByVideoInfoIdAsync(
                        this.VideoInfoId!.Value, paginationRequest, this.cancellationTokenSource.Token);
                    var result = GridItemsProviderResult.From(items!.Items!,
                    items.TotalItems);
                    this.IsBusy = false;
                    StateHasChanged();
                    return result;
                };
        }

        private async Task OnCreateVideoDailyPostAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                switch (this.SelectedSocialNetwork)
                {
                    case "LinkedIn":
                        this.DigitalMarketingDailyPostsIdeas =
                    await this.VideoDigitalMarketingDailyPostsService!
                    .CreateVideoDigitalMarketingDailyPostsForLinkedInAsync(this.VideoInfoId!.Value,
                        $"{this.SelectedCulture!.DisplayName}({this.SelectedCulture.Name})", this.cancellationTokenSource.Token);
                        break;
                    case "X":
                        this.DigitalMarketingDailyPostsIdeas =
                    await this.VideoDigitalMarketingDailyPostsService!
                    .CreateVideoDigitalMarketingDailyPostsForTwitterAsync(this.VideoInfoId!.Value,
                        $"{this.SelectedCulture!.DisplayName}({this.SelectedCulture.Name})", this.cancellationTokenSource.Token);
                        break;
                }
                await this.paginationState.SetCurrentPageIndexAsync(
                        this.paginationState.CurrentPageIndex);
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
            GC.SuppressFinalize(this);
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Video Daily Posts")]
        public const string VideoDailyPostsTextKey = "VideoDailyPostsText";
        [ResourceKey(defaultValue: "Create Video Daily Posts")]
        public const string CreateVideoDailyPostsTextKey = "CreateVideoDailyPostsText";
        #endregion Resource Keys
    }
}