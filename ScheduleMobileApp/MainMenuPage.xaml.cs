using ScheduleMobileApp.Models;
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
    public partial class MainMenuPage : ContentPage
    {
        private List<Frame> ButtonsFrameMenu = new List<Frame>();

        private bool PageLoadeded { get; set; } = false;

        private int CountForRowMiniMenu { get; } = 3;

        public MainMenuPage()
        {
            InitializeComponent();

        }

        private async void ContentPage_Appearing(object sender, EventArgs e)
        {
            if (!PageLoadeded)
            {
                await Task.Run(() =>
                {
                    InitMenu();
                });




                PageLoadeded = true;

                StackLayoutMenu.IsVisible = true;
            }
        }

        private void InitMenu()
        {
            InitButtonsMenu();

            StackLayoutMenu.Children.Clear();

            int column = 0;

            Grid gridRow = null;

            



            for(int i = 0; i < ButtonsFrameMenu.Count; i++)
            {
                var buttonFrameMenu = ButtonsFrameMenu[i];

                if (i % CountForRowMiniMenu == 0)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        gridRow = new Grid();

                        for (int g = 0; g < CountForRowMiniMenu; g++)
                        {
                            gridRow.ColumnDefinitions.Add(new ColumnDefinition());
                        }

                        StackLayoutMenu.Children.Add(gridRow);
                    });

                    
                    column = 0;
                }

                Grid.SetColumn(buttonFrameMenu, column);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    gridRow.Children.Add(buttonFrameMenu);
                });



                column++;
            }


        }

        private void InitButtonsMenu()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                ButtonsFrameMenu.Clear();
            });

            var tapGestureRecognizerCallsSchedule = new TapGestureRecognizer();
            tapGestureRecognizerCallsSchedule.Tapped += TapGestureRecognizerCallsSchedule_Tapped;
            ButtonsFrameMenu.Add(Services.Background.Worker.GetCellMenuMini("CallsSchedule.png", "Расписание звонков", tapGestureRecognizerCallsSchedule));


            var tapGestureRecognizerDatesNumeratorAndDenominatorSchedule = new TapGestureRecognizer();
            tapGestureRecognizerDatesNumeratorAndDenominatorSchedule.Tapped += TapGestureRecognizertapGestureRecognizerDatesNumeratorAndDenominatorSchedule_Tapped;
            ButtonsFrameMenu.Add(Services.Background.Worker.GetCellMenuMini("CallsSchedule.png", "Расписание числителя и знаменателя", tapGestureRecognizerDatesNumeratorAndDenominatorSchedule));

			var tapGestureRecognizerCallsScheduleExam = new TapGestureRecognizer();
			tapGestureRecognizerCallsScheduleExam.Tapped += TapGestureRecognizerCallsScheduleExam_Tapped;
			ButtonsFrameMenu.Add(Services.Background.Worker.GetCellMenuMini("CallsSchedule.png", "Расписание экзаменов", tapGestureRecognizerCallsScheduleExam));
		}

        private async void TapGestureRecognizerCallsScheduleExam_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CallsScheduleExamPage());
        }

		private async void TapGestureRecognizerCallsSchedule_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CallsSchedulePage());
        }
        private async void TapGestureRecognizertapGestureRecognizerDatesNumeratorAndDenominatorSchedule_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new DatesNumeratorAndDenominatorSchedulePage());
        }
    }
}