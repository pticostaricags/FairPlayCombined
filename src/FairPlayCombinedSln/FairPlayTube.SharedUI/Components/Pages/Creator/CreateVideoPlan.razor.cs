using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Interfaces.FairPlayTube;
using FairPlayCombined.Models.FairPlayTube.VideoPlan;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayTube.SharedUI.Components.Pages.Creator
{
    public partial class CreateVideoPlan
    {
        [SupplyParameterFromForm]
        private CreateVideoPlanModel CreateVideoPlanModel { get; set; } = new();
        [Inject] IAzureContentSafetyService? AzureContentSafetyService { get; set; }
        [Inject] IPromptGeneratorService? PromptGeneratorService { get; set; }
        [Inject] IOpenAIService? OpenAIService { get; set; }
        [Inject] IVideoPlanService? VideoPlanService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IUserProviderService? UserProviderService { get; set; }
        [Inject] IStringLocalizer<CreateVideoPlan>? Localizer { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private bool IsBusy { get; set; }
        private string? GeneratedYouTubeThumbnailUri { get; set; }
        private string? RevisedPrompt { get; set; }
        private bool IsPlanCreated { get; set; }

        protected override void OnInitialized()
        {
            this.CreateVideoPlanModel.ApplicationUserId = this.UserProviderService!.GetCurrentUserId();
        }

        private async Task<bool> IsValidInputAsync()
        {
            var videoNameModerationResult =
            await this.AzureContentSafetyService!.AnalyzeTextAsync(
                this.CreateVideoPlanModel.VideoName!, this.cancellationTokenSource.Token
            );
            if (videoNameModerationResult.IsOffensive || videoNameModerationResult.IsSexuallySuggestive ||
            videoNameModerationResult.IsSexuallyExplicity)
            {
                this.ToastService!.ShowError(Localizer![TitleNotAllowedTextKey]);
                return false;
            }

            var videoDescriptionModerationResult =
            await this.AzureContentSafetyService!.AnalyzeTextAsync(
                this.CreateVideoPlanModel.VideoDescription!, this.cancellationTokenSource.Token
            );
            if (videoDescriptionModerationResult.IsOffensive || videoDescriptionModerationResult.IsSexuallySuggestive ||
            videoDescriptionModerationResult.IsSexuallyExplicity)
            {
                this.ToastService!.ShowError(Localizer![DescriptionNotAllowedTextKey]);
                return false;
            }

            var videoScriptModerationResult =
            await this.AzureContentSafetyService!.AnalyzeTextAsync(
                this.CreateVideoPlanModel.VideoScript!, this.cancellationTokenSource.Token
            );
            if (videoScriptModerationResult.IsOffensive || videoScriptModerationResult.IsSexuallySuggestive ||
            videoScriptModerationResult.IsSexuallyExplicity)
            {
                this.ToastService!.ShowError(Localizer![ScriptNotAllowedTextKey]);
                return false;
            }

            return true;
        }

        private async Task OnValidSubmitAsync()
        {
            try
            {
                this.IsBusy = true;
                StateHasChanged();
                bool isValidInput = await IsValidInputAsync();
                if (isValidInput)
                {
                    var promptInfo = await this.PromptGeneratorService!.GetPromptCompleteInfoAsync(promptName:
                            "YouTubeThumbnail", cancellationToken: this.cancellationTokenSource.Token);
                    string prompt = $"{promptInfo!.BaseText}. Video Title: {this.CreateVideoPlanModel.VideoName}. Video Description: {this.CreateVideoPlanModel.VideoDescription}. Video Script: {this.CreateVideoPlanModel.VideoScript}";
                    if (prompt.Length > 4000)
                        prompt = prompt[..4000];
                    var promptShieldResponse = await this.AzureContentSafetyService!
                    .DetectJailbreakAttackAsync(new()
                    {
                        userPrompt = prompt
                    }, this.cancellationTokenSource.Token);
                    if (promptShieldResponse.userPromptAnalysis!.attackDetected)
                    {
                        this.ToastService!.ShowError(Localizer![ProhibitedContentTextKey]);
                    }
                    else
                    {
                        await CreateVideoPlanAsync(prompt);
                    }
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

        private async Task CreateVideoPlanAsync(string prompt)
        {
            await this.VideoPlanService!.CreateVideoPlanAsync(
                this.CreateVideoPlanModel, this.cancellationTokenSource.Token);
            this.ToastService!.ShowSuccess(Localizer![PlanCreatedTextKey]);
            StateHasChanged();
            this.IsPlanCreated = true;
            var result = await this.OpenAIService!.GenerateDallE3ImageAsync(prompt, this.cancellationTokenSource.Token);
            if (result != null)
            {
                this.GeneratedYouTubeThumbnailUri = result!.data![0]!.url!;
                this.RevisedPrompt = result!.data[0]!.revised_prompt;
                this.ToastService!.ShowSuccess(Localizer![ThumbnailCreatedTextKey]);
            }
            else
            {
                this.ToastService!.ShowError(Localizer![ThumbnailNotCreatedTextKey]);
            }
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "Create Video Plan")]
        public const string CreateVideoPlanTextKey = "CreateVideoPlanText";
        [ResourceKey(defaultValue: "Name")]
        public const string NameTextKey = "NameText";
        [ResourceKey(defaultValue: "Description")]
        public const string DescriptionTextKey = "DescriptionText";
        [ResourceKey(defaultValue: "Video Script")]
        public const string VideoScriptTextKey = "VideoScriptText";
        [ResourceKey(defaultValue: "Save")]
        public const string SaveTextKey = "SaveText";
        [ResourceKey(defaultValue: "Your Plan has been created")]
        public const string PlanCreatedTextKey = "PlanCreatedText";
        [ResourceKey(defaultValue: "You are not allowed to use that title, please use something more appropiate")]
        public const string TitleNotAllowedTextKey = "TitleNotAllowedText";
        [ResourceKey(defaultValue: "You are not allowed to use that description, please use something more appropiate")]
        public const string DescriptionNotAllowedTextKey = "DescriptionNotAllowedText";
        [ResourceKey(defaultValue: "You are not allowed to use that script, please use something more appropiate")]
        public const string ScriptNotAllowedTextKey = "ScriptNotAllowedText";
        [ResourceKey(defaultValue: "Prohibited content. Plan will not be created")]
        public const string ProhibitedContentTextKey = "ProhibitedContentText";
        [ResourceKey(defaultValue: "Your thumbnail has been created")]
        public const string ThumbnailCreatedTextKey = "ThumbnailCreatedText";
        [ResourceKey(defaultValue: "Your thumbnail could not be created")]
        public const string ThumbnailNotCreatedTextKey = "ThumbnailNotCreatedText";
        #endregion Resource Keys
    }
}