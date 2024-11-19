using FairPlayCombined.Common;
using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Common.GeneratorsAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.NewVideoRecommendation;
using FairPlayCombined.Models.Pagination;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class NewVideoRecommendation
    {
        [Inject] IJSRuntime? JsRuntime { get; set; }
        [Inject] IVideoInfoService? VideoInfoService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] INewVideoRecommendationService? NewVideoRecommendationService { get; set; }
        [Inject] IStringLocalizer<NewVideoRecommendation>? Localizer { get; set; }
        private bool IsBusy { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private string? NewVideoRecommendationIdea { get; set; }
        private GridItemsProvider<NewVideoRecommendationModel>? ItemsProvider { get; set; }
        private readonly PaginationState paginationState = new()
        {
            ItemsPerPage = Constants.Pagination.PageSize
        };

        protected override void OnInitialized()
        {
            base.OnInitialized();
            if (this.ItemsProvider is null)
            {
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
                            PropertyName=nameof(NewVideoRecommendationModel.NewVideoRecommendationId),
                            SortType= SortType.Descending
                        }
                        ]
                    };
                    var items = await this.NewVideoRecommendationService!
                    .GetPaginatedNewVideoRecommendationForUserIdAsync(
                        paginationRequest, this.UserProviderService!.GetCurrentUserId()!, this.cancellationTokenSource.Token);
                    var result = GridItemsProviderResult.From(items!.Items!, items.TotalItems);
                    this.IsBusy = false;
                    StateHasChanged();
                    return result;
                };
            }
        }

        private async Task OnCreateNewVideoRecommendationButtonClickedAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                this.NewVideoRecommendationIdea =
                    await NewVideoRecommendationService!
                    .GenerateNewVideoRecommendationAsync(CultureInfo.CurrentCulture.Name, this.cancellationTokenSource.Token);
                await this.paginationState.SetCurrentPageIndexAsync(this.paginationState.CurrentPageIndex);
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
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "New Video Recommendations")]
        public const string NewVideoRecommendationsTextKey = "NewVideoRecommendationsText";
        [ResourceKey(defaultValue: "Create New Recommendation")]
        public const string CreateNewRecommendationTextKey = "CreateNewRecommendationText";
        [ResourceKey(defaultValue: "Expand to see the content")]
        public const string ExpandToSeeContentTextKey = "ExpandToSeeContentText";
        #endregion Resource Keys
    }
}