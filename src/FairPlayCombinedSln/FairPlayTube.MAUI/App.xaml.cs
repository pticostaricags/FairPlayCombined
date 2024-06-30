using FairPlayTube.ClientServices.KiotaClient;

namespace FairPlayTube.MAUI
{
    public partial class App : Application
    {
        public App([FromKeyedServices("AnonymousApiClient")] ApiClient apiClient)
        {
            InitializeComponent();

            MainPage = new MauiPages.Login(apiClient);
        }
    }
}
