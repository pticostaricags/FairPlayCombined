using FairPlayTube.ClientServices.KiotaClient;

namespace FairPlayTube.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage()) { Title = nameof(FairPlayCombined.Common.Constants.ApplicationTitles.FairPlayTube) };
        }
    }
}
