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
	public partial class CallsScheduleExamPage : ContentPage
	{
		public CallsScheduleExamPage()
		{
			InitializeComponent();
		}

		private void ContentPage_Appearing(object sender, EventArgs e)
		{
			Init();
		}

		private void Init()
		{
			InitCellsScheduleExams();
		}

		private void InitCellsScheduleExams()
		{
			Task.Run(async() =>
			{
				bool isData = false;

				var group = Services.DataBase.Worker.GetSettingsCurrentGroup();
				var cellsScheduleExams = await Services.Background.Worker.GetCellsScheduleExamsForGroupAsync(group.Id);

				if (cellsScheduleExams != null)
				{
					if (cellsScheduleExams.Length > 0)
					{
						isData = true;
					}

					foreach (var cellScheduleExam in cellsScheduleExams)
					{
						Frame frameContainerCellScheduleExam = Services.Background.Worker.GetFrameContainerCellScheduleExam(cellScheduleExam);

						MainThread.BeginInvokeOnMainThread(() =>
						{
							CellsScheduleExamsStackLayout.Children.Add(frameContainerCellScheduleExam);
						});
					}
				}

				if (!isData)
				{
					MainThread.BeginInvokeOnMainThread(() =>
					{
						var labelNoData = new Label()
						{
							Text = "Нет данных",
							HorizontalOptions = LayoutOptions.Center,
							FontSize = 20,
						};

						labelNoData.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

						CellsScheduleExamsStackLayout.Children.Add(labelNoData);
					});
				}

			});
		}
	}
}