using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ScheduleMobileApp.Models;
using Newtonsoft.Json;
using System.Threading;
using Xamarin.Essentials;

namespace ScheduleMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {

        public static List<CellSchedule> DataCellsSchedule = new List<CellSchedule>();

        private DateTime DateForCellSchedule { get; set; }
        internal DateTime SelectDateForCellSchedule
        {
            get
            {
                return DateForCellSchedule;
            }
            set
            {
                DateForCellSchedule = value;
                UpdateDataPage();
            }
        }
        internal Models.DateByNumeratorAndDenominator CellScheduleType { get; set; }


        public SchedulePage()
        {
            InitializeComponent();

            InitSchedule();

            

        }

        private void InitSchedule()
        {
            Services.Background.Worker.SchedulePage = this;
            SelectDateForCellSchedule = DateTime.Now;
          
        }

        private async void UpdateDataPage()
        {
            TopMenuDayOfWeekAndDate.IsEnabled = false;
            CellsScheduleStackLayout.IsVisible = false;

            try
            {
                await Task.Run(async () =>
                {
                    UploadDayOfWeekAndDateTopMenuCellSchedule();

                    UploadScheduleAsync(SelectDateForCellSchedule, CellsScheduleStackLayout);

                    await UploadCellScheduleTypeAsync();

                    UploadTypeOfWeekContainer();
                });
            }
            catch(Exception error)
            {

            }

            //RefreshViewSchedule.IsRefreshing = false;
            CellsScheduleStackLayout.IsVisible = true;
            TopMenuDayOfWeekAndDate.IsEnabled = true;
        }

        private async Task<bool> UploadCellScheduleTypeAsync()
        {
            CellScheduleType = await Services.Background.Worker.GetCellScheduleTypeAsync(SelectDateForCellSchedule);

            return true;
        }

        private void UploadTypeOfWeekContainer()
        {

            if (Services.DataBase.Worker.GetSettingsShowTypeOfWeek())
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ContainerTypeOfWeek.IsVisible = true;
                    LabelTypeOfWeek.Text = Services.Background.Worker.GetStringForTypeOfWeek(CellScheduleType);
                });
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ContainerTypeOfWeek.IsVisible = false;
                });
            }
        }

        private void UploadDayOfWeekAndDateTopMenuCellSchedule()
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        Grid menu = Services.Background.Worker.GetDayOfWeekAndDateTopMenuCellSchedule(SelectDateForCellSchedule);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            TopMenuDayOfWeekAndDate.Content = menu;
                        });
                    }
                    catch (Exception error)
                    {
                        ScheduleMobileApp.Services.Background.Worker.ErrorNotify(error);
                    }
                });
            }
            catch (Exception error)
            {
                ScheduleMobileApp.Services.Background.Worker.ErrorNotify(error);
            }
        }

        private async void UploadScheduleAsync(DateTime date, StackLayout stackLayout)
        {
            try
            {
                
                
                    try
                    {
                        Group? group = ScheduleMobileApp.Services.DataBase.Worker.GetSettingsCurrentGroup();

                        if (group != null)
                        {
                            CellSchedule[] cellsSchedule = await Services.Background.Worker.GetScheduleByGroup(group, date);

                            if (cellsSchedule != null)
                            {
                                Services.Background.Worker.FillCellsSchedule(ref DataCellsSchedule, SelectDateForCellSchedule.DayOfWeek, stackLayout, cellsSchedule);
                            }


                        }
                    }
                    catch (Exception error)
                    {
                        ScheduleMobileApp.Services.Background.Worker.ErrorNotify(error);
                    }

                    




            }
            catch (Exception error)
            {
                ScheduleMobileApp.Services.Background.Worker.ErrorNotify(error);
            }


        }

        private void RefreshViewSchedule_Refreshing(object sender, EventArgs e)
        {
            UpdateDataPage();
        }
    }
}