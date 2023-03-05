using System;
using System.Collections.Generic;
using System.Text;

namespace ScheduleMobileApp
{
    internal class Config
    {
        static protected internal string UrlDeveloperWebSite { get; } = "http://derideveloper.ru"; 


        static protected internal string UrlWeb { get; set; }
        static protected internal string UrlWebApi { get; set; }
        static protected internal string UrlWebImages { get; set; }
        static protected internal double MobileAppVersion { get; set; } = 1.003;

        internal static void ModeWebUrl(bool typeServer)
        {
            if (typeServer)
            {
				UrlWeb = "http://192.168.1.112:7040";
			}
			else
            {
				UrlWeb = "http://derideveloper.ru:7040";
			}

			UrlWebApi = $"{UrlWeb}/api/";
			UrlWebImages = $"{UrlWeb}/images/";
		}

        static Config()
        {
            ModeWebUrl(false);
		}
    }
}
