using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScheduleMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedPageMain : TabbedPage
    {
        public TabbedPageMain()
        {
            InitializeComponent();

            InitMenu();
        }

        private void InitMenu()
        {

            SchedulePage schedulePage = new SchedulePage()
            {
                Title = "Расписание",
                IconImageSource = ImageSource.FromResource("ScheduleMobileApp.Images.home.png")
            };

            this.Children.Add(schedulePage);

            MainMenuPage mainMenuPage = new MainMenuPage()
            {
                Title = "Меню",
                IconImageSource = ImageSource.FromResource("ScheduleMobileApp.Images.menu.png")
            };

            this.Children.Add(mainMenuPage);


            SettingsPage settingsPage = new SettingsPage()
            {
                Title = "Настройки",
                IconImageSource = ImageSource.FromResource("ScheduleMobileApp.Images.settings.png")
            };

            this.Children.Add(settingsPage);



            this.CurrentPage = schedulePage;

        }
    }
}