using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class VideoDigitalMarketing
    {
        [Parameter]
        public long? VideoInfoId { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IVideoDigitalMarketingPlanService? VideoDigitalMarketingPlanService { get; set; }
        [Inject] ICultureService? CultureService { get; set; }
        [Inject] ICustomCache? CustomCache { get; set; }
        [Inject] IStringLocalizer<VideoDigitalMarketing>? Localizer { get; set; }
        private VideoInfoModel? VideoInfoModel { get; set; }
        private CultureInfo[]? SupportedCultures { get; set; }
        private CultureInfo? SelectedCulture { get; set; }
        private bool IsBusy { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private List<string>? DigitalMarketingIdeas { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            this.IsBusy = true;
            StateHasChanged();
            this.VideoInfoModel = await this.VideoInfoService!.GetVideoInfoByIdAsync(id: this.VideoInfoId!.Value,
            cancellationToken: this.cancellationTokenSource.Token);
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
            this.DigitalMarketingIdeas = (await this.VideoDigitalMarketingPlanService!
            .GetVideoDigitalMarketingPlansAsync(
                videoInfoId: this.VideoInfoModel.VideoInfoId,
                socialNetworkName: "LinkedIn",
                cancellationToken: this.cancellationTokenSource.Token))?.ToList();
            if (this.DigitalMarketingIdeas is null)
            {
                await this.GenerateDigitalMarketingIdeasAsync();
            }
            this.IsBusy = false;
            StateHasChanged();
        }

        private async Task OnRecreatePlanButtonclickedAsync()
        {
            this.IsBusy = true;
            StateHasChanged();
            await this.GenerateDigitalMarketingIdeasAsync();
            this.IsBusy = false;
            StateHasChanged();
        }

        private async Task GenerateDigitalMarketingIdeasAsync()
        {
            var result =
            await this.VideoDigitalMarketingPlanService!
            .CreateVideoDigitalMarketingPlanAsync(this.VideoInfoId!.Value,
                this.SelectedCulture!.DisplayName!,
                this.cancellationTokenSource.Token);
            this.DigitalMarketingIdeas?.Insert(0, result);
            StateHasChanged();
        }
        public async ValueTask DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Video Digital Marketing Strategy")]
        public const string VideoDigitalMarketingStrategyTextKey = "VideoDigitalMarketingStrategyText";
        [ResourceKey(defaultValue: "Create New Plan")]
        public const string CreateNewPlanTextKey = "CreateNewPlanText";
        #endregion Resource Keys
    }
}