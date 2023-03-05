using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ScheduleMobileApp.Models;
using Xamarin.Essentials;

namespace ScheduleMobileApp.Services.DataBase
{
    internal class Worker
    {
        static string nameSettings = "settings";

        internal static void UpdateSettingsGroup(Models.Group group)
        {
            try
            {
                if (group == null)
                    return;

                var settings = GetSettings();

                settings.CurrentGroup = group;

                UpdateSettings(settings);

            }
            catch(Exception error)
            {

            }
        }

        internal static Models.Group GetSettingsCurrentGroup()
        {
            var settings = GetSettings();

            return settings.CurrentGroup;
        }

        private static void UpdateSettings(Settings settings)
        {
            string settingsString = JsonConvert.SerializeObject(settings);

            Preferences.Set(nameSettings, settingsString);

        }

        private static Models.Settings GetSettings()
        {
            if (Preferences.ContainsKey(nameSettings))
            {
                var settingsString = Preferences.Get(nameSettings, null);

                var settings = JsonConvert.DeserializeObject<Models.Settings>(settingsString);

                return settings;
            }
            else
            {
                return new Models.Settings();
            }
        }

        internal static void UpdateSettingsShowCellId(bool value)
        {
            try
            {
                var settings = GetSettings();

                settings.ShowCellId = value;

                UpdateSettings(settings);

            }
            catch (Exception error)
            {

            }
        }
        
        internal static void UpdateSettingsShowTypeOfWeek(bool value)
        {
            try
            {
                var settings = GetSettings();

                settings.ShowTypeOfWeek = value;

                UpdateSettings(settings);

            }
            catch (Exception error)
            {

            }
        }

		internal static void UpdateSettingsTypeServer(bool value)
		{
			try
			{
				var settings = GetSettings();

				settings.TypeServer = value;

				UpdateSettings(settings);

			}
			catch (Exception error)
			{

			}
		}

		internal static void UpdateSettingsThemeApp(bool value)
        {
			try
			{
				var settings = GetSettings();

				settings.ThemeApp = value;

				UpdateSettings(settings);

			}
			catch (Exception error)
			{

			}
		}

        internal static bool GetSettingsThemeApp()
        {
            var settings = GetSettings();

            return settings.ThemeApp;
        }
        internal static bool GetSettingsTypeServer()
        {
            var settings = GetSettings();

            return settings.TypeServer;
        }

        internal static bool GetSettingsShowCellId()
        {
            var settings = GetSettings();

            return settings.ShowCellId;
        }

        internal static bool GetSettingsShowTypeOfWeek()
        {
            var settings = GetSettings();

            return settings.ShowTypeOfWeek;
        }

		
		internal static void UpdateSettingsModeDeveloper(bool status)
		{
			try
			{
				var settings = GetSettings();

				settings.ModeDeveloper = status;

				UpdateSettings(settings);

			}
			catch (Exception error)
			{

			}
		}

		internal static bool GetSettingsModeDeveloper()
		{
			var settings = GetSettings();

			return settings.ModeDeveloper;
		}

		internal static void UpdateSettingsAppVersion(double appVersion)
		{
			try
			{
				var settings = GetSettings();

				settings.AppVersion = appVersion;

				UpdateSettings(settings);

			}
			catch (Exception error)
			{

			}
		}
		internal static double GetSettingsAppAndroidVersion()
		{
			var settings = GetSettings();

			return settings.AppVersion;
		}

		internal static double GetSettingsAppVersion()
		{
			var settings = GetSettings();

			return settings.AppVersion;
		}
	}
}
