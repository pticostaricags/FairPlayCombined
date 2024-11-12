using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoPlan;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class MyVideoPlans
    {
        [Inject] IVideoPlanService? VideoPlanService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] ILogger<MyVideoPlans>? Logger { get; set; }
        [Inject] IStringLocalizer<MyVideoPlans>? Localizer { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool IsBusy { get; set; }
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };
        private GridItemsProvider<VideoPlanModel>? itemsProvider;

        protected override void OnInitialized()
        {
            this.itemsProvider = async req =>
            {
                this.IsBusy = true;
                StateHasChanged();
                var items = await this.VideoPlanService!.GetPaginatedVideoPlanAsync(paginationRequest: new()
                {
                    PageSize = paginationState.ItemsPerPage
                }, cancellationToken: this.cancellationTokenSource.Token);
                this.IsBusy = false;
                StateHasChanged();
                var result = GridItemsProviderResult.From<VideoPlanModel>(items!.Items!, items.TotalItems);
                return result;
            };
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "My Video Plans")]
        public const string MyVideoPlansTextKey = "MyVideoPlansText";
        [ResourceKey(defaultValue: "Edit")]
        public const string EditTextKey = "EditText";
        [ResourceKey(defaultValue: "Video Name")]
        public const string VideoNameTextKey = "VideoNameText";
        [ResourceKey(defaultValue: "Video Description")]
        public const string VideoDescriptionTextKey = "VideoDescriptionText";
        [ResourceKey(defaultValue: "Video Script")]
        public const string VideoScriptTextKey = "VideoScriptText";
        #endregion Resource Keys
    }
}