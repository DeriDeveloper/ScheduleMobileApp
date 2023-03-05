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
    public partial class DatesNumeratorAndDenominatorSchedulePage : ContentPage
    {
        public DatesNumeratorAndDenominatorSchedulePage()
        {
            InitializeComponent();
        }

        private void ContentPage_Appearing(object sender, EventArgs e)
        {
            Init();
        }

        private void Init()
        {
            UploadDateByNumeratorAndDenominatorSchedule();
        }

        private async void UploadDateByNumeratorAndDenominatorSchedule()
        {
            StackLayoutDateByNumeratorAndDenominatorSchedule.IsVisible = false;
            StackLayoutDateByNumeratorAndDenominatorSchedule.Children.Clear();

            await Task.Run(async () =>
            {
                Models.DateByNumeratorAndDenominator[]? datesNumeratorAndDenominator = await Services.Background.Worker.GetDatesNumeratorAndDenominatorAsync();


                if (datesNumeratorAndDenominator != null)
                {
                    foreach (var dateByNumeratorAndDenominator in datesNumeratorAndDenominator)
                    {
                        var frameContainer = Services.Background.Worker.GetFrameContainerDateByNumeratorAndDenominator(dateByNumeratorAndDenominator);

                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            StackLayoutDateByNumeratorAndDenominatorSchedule.Children.Add(frameContainer);
                        });
                    }
                }
            });

            StackLayoutDateByNumeratorAndDenominatorSchedule.IsVisible = true;
        }
    }
}