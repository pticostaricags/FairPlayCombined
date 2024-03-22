using Blazored.Toast.Services;
using FairPlayCombined.Common.FairPlaySocial;
using FairPlayCombined.Interfaces;
using FairPlayCombined.Models.FairPlaySocial.Notification;
using FairPlaySocial.ClientServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR.Client;

namespace FairPlaySocial.Components.Notifications
{
    public partial class UserMessage
    {
        [Inject]
        private IToastService? ToastService { get; set; }
        [Inject]
        private NavigationManager? NavigationManager { get; set; }
        private HubConnection? HubConnection { get; set; }
        private bool IsBusy { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                this.IsBusy = true;
                var hubUrl = this.NavigationManager!
                    .ToAbsoluteUri($"{Constants.Hubs.UserMessageHub}");                
                this.HubConnection = new HubConnectionBuilder()
                    .WithUrl(hubUrl)
                    .Build();
                this.HubConnection.On(Constants.Hubs.ReceiveMessage,
                    (Action<UserMessageNotificationModel>)((model) =>
                    {
                        ToastService!
                            .ShowSuccess($"You have received a new message: {model.Message}");
                        StateHasChanged();
                    }));
                await this.HubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                this.ToastService!.ShowError(ex.Message);
            }
            finally
            {
                this.IsBusy = false;
            }
        }
    }
}
