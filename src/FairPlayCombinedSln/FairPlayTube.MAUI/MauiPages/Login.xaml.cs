using FairPlayTube.ClientServices.KiotaClient;
using FairPlayTube.ClientServices.KiotaClient.Models;
using FairPlayTube.MAUI.Authentication;
using Microsoft.Kiota.Abstractions;
using System.Net;

namespace FairPlayTube.MAUI.MauiPages;

public partial class Login : ContentPage
{
    private ApiClient apiClient;

    public Login([FromKeyedServices("AnonymousApiClient")] ApiClient apiClient)
	{
		InitializeComponent();
        this.apiClient = apiClient;
    }

    private async void btnLogin_Clicked(object sender, EventArgs e)
    {
        try
        {
            LoginRequest loginRequest = new()
            {
                Email = this.txtUsername!.Text,
                Password = this.txtPassword!.Text,
            };
            var result = await this.apiClient!.Login.PostAsync(loginRequest);
            if (!String.IsNullOrWhiteSpace(result!.AccessToken))
            {
                UserContext.AccessToken = result.AccessToken;
                UserContext.AccessTokenExpiresIn = result.ExpiresIn;
                UserContext.RefreshToken = result.RefreshToken;
                UserContext.TokenExpiraton = DateTimeOffset.UtcNow.AddMinutes(result!.ExpiresIn!.Value);
                App.Current!.MainPage=new MainPage();
            }
            else
            {
                await Application.Current!.Windows[0].Page!.DisplayAlert("Error", "Unknown error", "OK");
            }
        }
        catch (ApiException apiEx)
        {
            if (apiEx.ResponseStatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await Application.Current!.Windows[0].Page!.DisplayAlert("Unable to login",
                "Unable to login, please make soure you are using the correct credentials",
                "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current!.Windows[0].Page!.DisplayAlert("Error", ex.Message, "OK");
        }
    }
}