using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlayTube.VideoInfo;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Globalization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class VideoPassiveIncome
    {
        [Parameter]
        public long? VideoInfoId { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IVideoPassiveIncomeIdeaService? VideoPassiveIncomeIdeaService { get; set; }
        [Inject] ICustomCache? CustomCache { get; set; }
        [Inject] ICultureService? CultureService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IStringLocalizer<VideoPassiveIncome>? Localizer { get; set; }

        private VideoInfoModel? VideoInfoModel { get; set; }
        private CultureInfo[]? SupportedCultures { get; set; }
        private CultureInfo? SelectedCulture { get; set; }
        private bool IsBusy { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private List<string>? PassiveIncomeIdeas { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
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
            this.PassiveIncomeIdeas = (await this.VideoPassiveIncomeIdeaService!
            .GetVideoPassiveIncomeIdeasAsync(
                videoInfoId: this.VideoInfoId!.Value!,
                cancellationToken: this.cancellationTokenSource.Token))?.ToList();
            this.IsBusy = false;
            StateHasChanged();
        }

        private async Task OnCreatePassiveIncomeIdeaButtonClickedAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                var result = await this.VideoPassiveIncomeIdeaService!.CreateVideoPassiveIncomeIdeaAsync(
                this.VideoInfoId!.Value, this.SelectedCulture!.DisplayName!, this.cancellationTokenSource.Token);
                this.PassiveIncomeIdeas?.Insert(0, result);
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
        [ResourceKey(defaultValue: "Video Passive Income")]
        public const string VideoPassiveIncomeTextKey = "VideoPassiveIncomeTextKey";
        [ResourceKey(defaultValue: "Create Passive Income Idea")]
        public const string CreatePassiveIncomeIdeaTextKey = "CreatePassiveIncomeIdeaText";
        #endregion Resource Keys
    }
}