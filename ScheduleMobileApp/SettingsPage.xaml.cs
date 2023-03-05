using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ScheduleMobileApp.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScheduleMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        private bool InitStatus { get; set; }

        public SettingsPage()
        {
            InitializeComponent();
        }

        

        private async void InitPickerGroupsAsync()
        {
            try
            {
                ScheduleMobileApp.Models.Group[] groups = await Services.Background.Worker.GetAllGroups();

                if (groups != null)
                {
                    PickerGroupSelect.ItemsSource = new BindingList<ScheduleMobileApp.Models.Group>(groups);
                }

                var currentGroup = Services.DataBase.Worker.GetSettingsCurrentGroup();

                if (currentGroup != null) {
                    var indexGroup = -1;

                    var tempGroups = (BindingList<Group>)PickerGroupSelect.ItemsSource;

                    if (tempGroups != null)
                    {
                        for (int indexTemp = 0; indexTemp < tempGroups.Count; indexTemp++)
                        {
                            if (tempGroups[indexTemp].Id == currentGroup.Id)
                            {
                                indexGroup = indexTemp;
                                break;
                            }
                        }

                        if (indexGroup >= 0)
                        {
                            PickerGroupSelect.SelectedIndex = indexGroup;
                        }
                    }
                }
            }
            catch (Exception error)
            {
                ScheduleMobileApp.Services.Background.Worker.ErrorNotify(error);
            }
        }

        private void PickerGroupSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;

            var selectedGroup = (Group)picker.SelectedItem;

            if (selectedGroup != null)
            {
                Services.DataBase.Worker.UpdateSettingsGroup(selectedGroup);
                
            }
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            InitStatus = true;


			InitPickerGroupsAsync();


			var group = Services.DataBase.Worker.GetSettingsCurrentGroup();
            if (group != null)
            {
                var groupsList = (BindingList<Models.Group>)PickerGroupSelect.ItemsSource;

                if (groupsList != null) {
                    foreach (var groupTemp in groupsList)
                    {
                        if (groupTemp.Id == group.Id)
                        {
                            PickerGroupSelect.SelectedItem = groupTemp;
                            break;
                        }
                    }
                } 
            }

            var statusShowCellId = Services.DataBase.Worker.GetSettingsShowCellId();
            SwitchShowCellId.IsToggled = statusShowCellId;

            var statusShowTypeOfWeek = Services.DataBase.Worker.GetSettingsShowTypeOfWeek();
            SwitchShowTypeOfWeek.IsToggled = statusShowTypeOfWeek;

            var statusThemeApp = Services.DataBase.Worker.GetSettingsThemeApp();
            SwitchThemeApp.IsToggled = statusThemeApp;

			var statusTypeServer = Services.DataBase.Worker.GetSettingsTypeServer();
			SwitchTypeServer.IsToggled = statusTypeServer;

			var statusModeDeveloper = Services.DataBase.Worker.GetSettingsModeDeveloper();
			SwitchModeDeveloper.IsToggled = statusModeDeveloper;


            
            

            SetModeDeveloper(statusModeDeveloper);





            InitStatus = false;
        }

        private void SetModeDeveloper(bool status)
        {
            CellIdContainerStackLayout.IsVisible = status;
            TypeServerContainerStackLayout.IsVisible = status;
            ModeDeveloperContainerStackLayout.IsVisible = status;
			TypeOfWeekContainerStackLayout.IsVisible = status;
        }

		private void SwitchShowCellId_Toggled(object sender, ToggledEventArgs e)
        {
            if (!InitStatus)
            {
                Services.DataBase.Worker.UpdateSettingsShowCellId(e.Value);
            }
        }
        private void SwitchShowTypeOfWeek_Toggled(object sender, ToggledEventArgs e)
        {
            if (!InitStatus)
            {
                Services.DataBase.Worker.UpdateSettingsShowTypeOfWeek(e.Value);
            }
        }

        private void SwitchTypeServer_Toggled(object sender, ToggledEventArgs e)
        {
            if (!InitStatus)
            {
                Services.DataBase.Worker.UpdateSettingsTypeServer(e.Value);

                Config.ModeWebUrl(e.Value);
            }
        }

		private void SwitchThemeApp_Toggled(object sender, ToggledEventArgs e)
        {
            if (!InitStatus)
            {
                var statusThemeApp = e.Value;

                Services.DataBase.Worker.UpdateSettingsThemeApp(statusThemeApp);

                if (statusThemeApp)
                {
                    App.Current.UserAppTheme = OSAppTheme.Dark;
                }
                else
                {
                    App.Current.UserAppTheme = OSAppTheme.Light;
                }
            }
        }
		private void SwitchModeDeveloper_Toggled(object sender, ToggledEventArgs e)
		{
			if (!InitStatus)
			{
				Services.DataBase.Worker.UpdateSettingsModeDeveloper(e.Value);
                SetModeDeveloper(e.Value);
			}
		}

		private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AboutApplicationPage());
        }

        
	}
}