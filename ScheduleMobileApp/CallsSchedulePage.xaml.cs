using ScheduleMobileApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ScheduleMobileApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CallsSchedulePage : ContentPage
    {
        public CallsSchedulePage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            InitPickerDayOfWeek();
        }

        private void InitPickerDayOfWeek()
        {
            List<Models.ModelDayOfWeek> dayOfWeeks = new List<Models.ModelDayOfWeek>()
            {
                new Models.ModelDayOfWeek(){ Name = "Понедельник", DayOfWeek = DayOfWeek.Monday },
                new Models.ModelDayOfWeek(){ Name = "Вторник", DayOfWeek = DayOfWeek.Tuesday },
                new Models.ModelDayOfWeek(){ Name = "Среда", DayOfWeek = DayOfWeek.Wednesday },
                new Models.ModelDayOfWeek(){ Name = "Четверг", DayOfWeek = DayOfWeek.Thursday },
                new Models.ModelDayOfWeek(){ Name = "Пятница", DayOfWeek = DayOfWeek.Friday },
                new Models.ModelDayOfWeek(){ Name = "Суббота", DayOfWeek = DayOfWeek.Saturday },
                new Models.ModelDayOfWeek(){ Name = "Воскресенье", DayOfWeek = DayOfWeek.Sunday },
            };

            PickerDayOfWeek.ItemsSource = dayOfWeeks;

            PickerDayOfWeek.SelectedIndex = 0;
        }

        private async void PickerDayOfWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var pickerDayOfWeek = (Picker)sender;

                if (pickerDayOfWeek != null)
                {
                    var modelDayOfWeek = (Models.ModelDayOfWeek)pickerDayOfWeek.SelectedItem;

                    if (modelDayOfWeek != null)
                    {
                        var timesPairs = await Services.Background.Worker.GetTimesPairsAsync(modelDayOfWeek.DayOfWeek);

                        UploadCallsSchedule(timesPairs);
                    }
                }
            }
            catch(Exception error)
            {

            };
        }

        private void UploadCallsSchedule(TimesPair[] timesPairs)
        {
            StackLayoutCallsSchedule.IsVisible = false;
            StackLayoutCallsSchedule.Children.Clear();

            Task.Run(() => {
                
                foreach(var timesPair  in timesPairs)
                {
                    Frame frameContainerTimesPair = null;

                    if (timesPair.ChangeTimesPair != null)
                    {
                        //мб значок иконку поставить что изменен, и потом посмотреть при нажатии что какое было время (но это если вреям было и добавили изменение к ней)
                        frameContainerTimesPair = Services.Background.Worker.GetFrameContainerTimesPair(timesPair.ChangeTimesPair, timesPair);
                    }
                    else
                    {
                        frameContainerTimesPair = Services.Background.Worker.GetFrameContainerTimesPair(timesPair);
                    }


                    if (frameContainerTimesPair != null) {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            StackLayoutCallsSchedule.Children.Add(frameContainerTimesPair);
                        });
                    }
                }

            });


            StackLayoutCallsSchedule.IsVisible = true;
        }
    }
}