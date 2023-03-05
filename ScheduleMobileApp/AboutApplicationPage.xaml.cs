using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScheduleMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutApplicationPage : ContentPage
    {
        public AboutApplicationPage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            ImageDeveloperLogo.Source = ImageSource.FromUri(new Uri(Config.UrlWebImages + "developer/LogoFullDark.png"));

            AppVersionLabel.Text = Services.DataBase.Worker.GetSettingsAppVersion().ToString();
		}

        private async void TapGestureRecognizerCreatedAppDeveloper_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Browser.OpenAsync(new Uri(Config.UrlDeveloperWebSite), BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.ToString());
            }
        }

		private void ImageDeveloperLogoTapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
            Services.DataBase.Worker.UpdateSettingsModeDeveloper(true);
        }
    }
}