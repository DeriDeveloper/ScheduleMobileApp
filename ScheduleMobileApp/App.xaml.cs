using System;
using ScheduleMobileApp.Models;
using ScheduleMobileApp.Services.DataBase;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScheduleMobileApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			var statusThemeApp = Services.DataBase.Worker.GetSettingsThemeApp();
			if (statusThemeApp)
			{
				App.Current.UserAppTheme = OSAppTheme.Dark;
			}
			else
			{
				App.Current.UserAppTheme = OSAppTheme.Light;
			}

            Init();
		}

        protected override void OnStart()
        {
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        private async void Init()
        {
            Services.DataBase.Worker.UpdateSettingsAppVersion(Config.MobileAppVersion);

			Config.ModeWebUrl(Services.DataBase.Worker.GetSettingsTypeServer());


			Application.Current.MainPage = new TabbedPageMain();

            MobileApp mobileAppAndroid = null;

            try
            {
                var mobileApps = await Services.Background.Worker.GetMobileAppsAsync();

                if (mobileApps != null)
                {
                    foreach (var mobileApp in mobileApps)
                    {
                        if (mobileApp.Type == Types.Enums.TypeMobileApp.Android)
                        {

                            mobileAppAndroid = mobileApp;
                            break;
                        }
                    }

                    if (mobileAppAndroid != null)
                    {
                        double currentAndroidAppVersion = Services.DataBase.Worker.GetSettingsAppAndroidVersion();

                        if (mobileAppAndroid.Version > currentAndroidAppVersion)
                        {
                            await Browser.OpenAsync(Config.UrlWeb + "/MobileApplication");
                        }
                    }
                }
            }
            catch(Exception error)
            {

            }

		}
	}
}
