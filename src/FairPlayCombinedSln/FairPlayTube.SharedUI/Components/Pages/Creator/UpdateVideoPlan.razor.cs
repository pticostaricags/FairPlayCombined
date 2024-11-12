using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.AzureVideoIndexer;
using FairPlayCombined.Models.FairPlayTube.VideoPlan;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.FluentUI.AspNetCore.Components;
using FairPlayCombined.Common.CustomAttributes;
using Microsoft.Extensions.Localization;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class UpdateVideoPlan
    {
        [Parameter]
        public long? VideoPlanId { get; set; }
        [SupplyParameterFromForm]
        private UpdateVideoPlanModel updateVideoPlanModel { get; set; } = new();
        [Inject] IPromptGeneratorService? PromptGeneratorService { get; set; }
        [Inject] IOpenAIService? OpenAIService { get; set; }
        [Inject] IAzureVideoIndexerService? AzureVideoIndexerService { get; set; }
        [Inject] IVideoPlanService? VideoPlanService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] IYouTubeClientService? YouTubeClientService { get; set; }
        [Inject] ILogger<CreateVideoPlan>? Logger { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        [Inject] AzureVideoIndexerServiceConfiguration? AzureVideoIndexerServiceConfiguration { get; set; }
        [Inject] IStringLocalizer<UpdateVideoPlan>? Localizer { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool IsBusy { get; set; }
        private string? GeneratedYouTubeThumbnailUri { get; set; }
        private string? RevisedPrompt { get; set; }
        private VideoPlanModel? originalVideoPlan { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                this.updateVideoPlanModel.ApplicationUserId = this.UserProviderService!.GetCurrentUserId();
                if (this.originalVideoPlan is null)
                {
                    this.originalVideoPlan = await this.VideoPlanService!.GetVideoPlanByIdAsync(this.VideoPlanId!.Value, this.cancellationTokenSource.Token);
                    this.updateVideoPlanModel.VideoPlanId = this.VideoPlanId.Value;
                    this.updateVideoPlanModel.VideoName = this.originalVideoPlan.VideoName;
                    this.updateVideoPlanModel.VideoDescription = this.originalVideoPlan.VideoDescription;
                    this.updateVideoPlanModel.VideoScript = this.originalVideoPlan.VideoScript;
                }
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

        private async Task OnValidSubmitAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                await this.VideoPlanService!.UpdateVideoPlanAsync(
                    this.updateVideoPlanModel, this.cancellationTokenSource.Token);
                this.ToastService!.ShowSuccess(Localizer![PlanUpdatedTextKey]);
                StateHasChanged();
                var promptInfo = await this.PromptGeneratorService!.GetPromptCompleteInfoAsync(promptName:
                        "YouTubeThumbnail", cancellationToken: this.cancellationTokenSource.Token);
                string prompt = $"{promptInfo!.BaseText}. Video Title: {this.updateVideoPlanModel.VideoName}. Video Description: {this.updateVideoPlanModel.VideoDescription}. Video Script: {this.updateVideoPlanModel.VideoScript}";
                if (prompt.Length > 4000)
                    prompt = prompt.Substring(0, 4000);
                var result = await this.OpenAIService!.GenerateDallE3ImageAsync(prompt, this.cancellationTokenSource.Token);
                if (result != null)
                {
                    this.GeneratedYouTubeThumbnailUri = result!.data![0]!.url!;
                    this.RevisedPrompt = result!.data[0]!.revised_prompt;
                    this.ToastService!.ShowSuccess(Localizer![ThumbnailCreatedTextKey]);
                }
                else
                {
                    this.ToastService!.ShowError(Localizer![ThumbnailWasNotCreatedTextKey]);
                }
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
        [ResourceKey(defaultValue: "Update Video Plan")]
        public const string UpdateVideoPlanTextKey = "UpdateVideoPlanText";
        [ResourceKey(defaultValue: "Name")]
        public const string NameTextKey = "NameText";
        [ResourceKey(defaultValue: "Description")]
        public const string DescriptionTextKey = "DescriptionText";
        [ResourceKey(defaultValue: "Video Script")]
        public const string VideoScriptTextKey = "VideoScriptText";
        [ResourceKey(defaultValue: "Save")]
        public const string SaveTextKey = "SaveText";
        [ResourceKey(defaultValue: "Your Plan has been updated")]
        public const string PlanUpdatedTextKey = "PlanUpdatedText";
        [ResourceKey(defaultValue: "Your thumbnail has been created")]
        public const string ThumbnailCreatedTextKey = "ThumbnailCreatedTextKey";
        [ResourceKey(defaultValue: "Your thumbnail could not be created")]
        public const string ThumbnailWasNotCreatedTextKey = "ThumbnailWasNotCreatedText";
        #endregion Resource Keys
    }
}