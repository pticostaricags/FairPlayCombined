using FairPlayCombined.Common.CustomAttributes;
using FairPlayCombined.Interfaces.Common;
using FairPlayCombined.Models.PayPal;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.FluentUI.AspNetCore.Components;

namespace FairPlayTube.SharedUI.Components.Pages.User
{
    public partial class MyFunds
    {
        [Inject] IUserFundService? UserFundService { get; set; }
        [Inject] IToastService? ToastService { get; set; }
        [Inject] IUserFundsUniqueCodesService? UserFundsUniqueCodesService { get; set; }
        [Inject] IStringLocalizer<MyFunds>? Localizer { get; set; }
        private decimal? AvailableFunds { get; set; }
        private readonly CancellationTokenSource cancellationTokenSource = new();
        private string? CodeToClaim { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
        }

        private async Task LoadDataAsync()
        {
            this.AvailableFunds = await this.UserFundService!
            .GetMyAvailableFundsAsync(this.cancellationTokenSource.Token);
            StateHasChanged();
        }

        private async Task OnPayPalOrderApprovedAsync(
            (
                PaypalCheckoutApprovedDataModel data,
                PaypalCheckoutApprovedDetailsModel details
            ) tuple
        )
        {
            try
            {
                await this.UserFundService!.AddMyFundsAsync(tuple.data.orderID!, this.cancellationTokenSource.Token);
                this.ToastService!.ShowSuccess($"Funds have been added to your Wallet");
                await LoadDataAsync();
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
        }

        private async Task OnClaimCodeClickAsync()
        {
            try
            {
                Guid codeGuid = Guid.Parse(this.CodeToClaim!);
                await this.UserFundsUniqueCodesService!.ClaimFundsUniqueCodeAsync(codeGuid, this.cancellationTokenSource.Token);
                this.ToastService!.ShowInfo("Your code has been claimed and added to your funds");
                this.CodeToClaim = String.Empty;
                await this.LoadDataAsync();
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
        }

        private void OnError(string errorMessage)
        {
            this.ToastService!.ShowError(errorMessage);
        }

        public async ValueTask DisposeAsync()
        {
            await this.cancellationTokenSource.CancelAsync();
            this.cancellationTokenSource.Dispose();
        }

        #region Resource Keys
        [ResourceKey(defaultValue: "My Funds")]
        public const string MyFundsTextKey = "MyFundsText";
        [ResourceKey(defaultValue: "Available Funds")]
        public const string AvailableFundsTextKey = "AvailableFundsText";
        [ResourceKey(defaultValue: "Use Code")]
        public const string UseCodeTextKey = "UseCodeText";
        [ResourceKey(defaultValue: "Claim Code")]
        public const string ClaimCodeTextKey = "ClaimCodeText";
        #endregion Resource Keys
    }
}