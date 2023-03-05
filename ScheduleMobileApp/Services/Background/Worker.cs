using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ScheduleMobileApp.Models;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using ScheduleMobileApp.Interfaces;
using ScheduleMobileApp.Types;
using static ScheduleMobileApp.Types.Enums;

namespace ScheduleMobileApp.Services.Background
{
    internal class Worker
    {
        internal static List<StackLayout> StackLayoutsForCellsScheduleMainWindow = new List<StackLayout>();
        internal static SchedulePage SchedulePage { get; set; }

        static internal void ErrorNotify(Exception exception)
        {
            string error = DateTime.Now.ToString() + exception.ToString();

            Console.WriteLine(error);
        }

        internal static void FillCellsSchedule(ref List<CellSchedule> dataCellsSchedule, DayOfWeek selectedDayOfWeek, StackLayout cellsScheduleStackLayout, CellSchedule[] cellsSchedule)
        {
            try
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    cellsScheduleStackLayout.Children.Clear();
                });

                dataCellsSchedule.Clear();

                if (cellsSchedule != null)
                {
                    foreach (var cellSchedule in cellsSchedule)
                    {
                        dataCellsSchedule.Add(cellSchedule);
                    }
                }
                


                if (dataCellsSchedule != null)
                {
                    foreach (var cellSchedule in dataCellsSchedule)
                    {
                        if (cellSchedule != null)
                        {
                            Frame cellScheduleContent = GetCellScheduleFrame(cellSchedule);

                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                cellsScheduleStackLayout.Children.Add(cellScheduleContent);
                            });
                        }
                    }
                }



                // тут надо добавить дефолтные ячейки (кур час) поднятие и опускание  флага

                var scheduleCellsAdditionalInformation = GetScheduleCellsAdditionalInformationAsync().Result;

                if (scheduleCellsAdditionalInformation != null)
                {
                    foreach (var scheduleCellAdditionalInformation in scheduleCellsAdditionalInformation)
                    {
                        if (selectedDayOfWeek == scheduleCellAdditionalInformation.DayOfWeek)
                        {
                            int indexInsertCellScheduleContent = 0;

                            Services.Background.Worker.GetIndexInsertCellScheduleByAfterNumberOfPair(ref dataCellsSchedule, scheduleCellAdditionalInformation.AfterNumberOfPair, out indexInsertCellScheduleContent);

                            dataCellsSchedule.Insert(indexInsertCellScheduleContent, new CellSchedule()
                            {
                                //NumberPair = scheduleCellAdditionalInformation.AfterNumberOfPair,
                            });


                            Frame cellScheduleContent = GetCellScheduleAdditionalInformationFrame(scheduleCellAdditionalInformation.TimesPair, scheduleCellAdditionalInformation.Name);


                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                // тут найти позицию
                                cellsScheduleStackLayout.Children.Insert(indexInsertCellScheduleContent, cellScheduleContent);
                                
                            });
                        }
                    }
                }

                //----------------




                

            }
            catch (Exception error)
            {

            }
        }

        private static CellSchedule? GetCellScheduleByNumberPair(List<CellSchedule> dataCellsSchedule, int numberPair, out int indexCellScheduleOld)
        {
            indexCellScheduleOld = 0;
            CellSchedule cellSchedule = null;

            for (int i = 0; i < dataCellsSchedule.Count; i++)
            {
                var selectedCellSchedule = dataCellsSchedule[i];
                int selectedNumberPair = selectedCellSchedule.NumberPair;

                if(selectedNumberPair == numberPair)
                {
                    indexCellScheduleOld = i;
                    return selectedCellSchedule;
                }
            }

            return cellSchedule;
        }

        private static void GetIndexInsertCellScheduleByAfterNumberOfPair(ref List<CellSchedule> dataCellsSchedule, int afterNumberOfPair, out int indexInsertCellSchedule)
        {
            indexInsertCellSchedule = 0;

            try
            {
                


                for (int i = 0; i < dataCellsSchedule.Count; i++)
                {
                    int numberPair = dataCellsSchedule[i].NumberPair;

                    if (numberPair == afterNumberOfPair)
                    {
                        indexInsertCellSchedule = i + 1;
                        break;
                    }
                    else if (numberPair > afterNumberOfPair)
                    {
                        indexInsertCellSchedule = i;
                        break;
                    }
                    else if (numberPair < afterNumberOfPair)
                    {
                        indexInsertCellSchedule = i + 1;
                    }
                }


                
            }
            catch(Exception error)
            {
                Console.WriteLine(error.ToString());
            }
        }

        private static Frame GetCellScheduleFrame(CellSchedule cellSchedule)
        {
            Frame frameMainContainer = new Frame()
            {
                CornerRadius = 10,
                Padding = 0,
                Margin = 0,
                HasShadow = true,
            };

            frameMainContainer.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeDark"]);


            try
            {


                Grid gridMainContainer = new Grid()
                {
                    RowSpacing = 0
                };

                frameMainContainer.Content = gridMainContainer;


                Frame frameContainerCell = new Frame()
                {
                    CornerRadius = 0,
                    Padding = 10,
                    Margin = 0,
                    HasShadow = false,
                    BackgroundColor =Color.Transparent
                };


                gridMainContainer.Children.Add(frameContainerCell);






                StackLayout stackLayoutContainerCell = new StackLayout()
                {
                };

                frameContainerCell.Content = stackLayoutContainerCell;


                Grid gridTopContainerCell = new Grid()
                {
                    ColumnDefinitions =
                    {
                         new ColumnDefinition() {Width = new GridLength(80, GridUnitType.Absolute)},
                         new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)}
                    },
                };

                Grid gridBottomContainerCell = new Grid()
                {
                    ColumnDefinitions =
                    {
                         new ColumnDefinition() {Width = new GridLength(80, GridUnitType.Absolute)},
                         new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)}
                    },
                };

                stackLayoutContainerCell.Children.Add(gridTopContainerCell);
                stackLayoutContainerCell.Children.Add(gridBottomContainerCell);


                



                //Время пары контейнер
                StackLayout stackLayoutTimesContainer = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions = LayoutOptions.Start
                };

                Grid.SetColumn(stackLayoutTimesContainer, 0);

                gridTopContainerCell.Children.Add(stackLayoutTimesContainer);

                Image imageClock = new Image
                {
                    Source = ImageSource.FromResource("ScheduleMobileApp.Images.clock.png"),
                    WidthRequest = 15,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };

                stackLayoutTimesContainer.Children.Add(imageClock);


                StackLayout stackLayoutTimeContent = new StackLayout()
                {
                   Spacing = 0,
                };

                stackLayoutTimesContainer.Children.Add(stackLayoutTimeContent);

                Label TimeStartPairLabel = new Label()
                {
                    Text = cellSchedule.TimesPair.TimeStart.ToString("HH:mm"),
                    Padding = 0,
                    Margin = 0
                };
                TimeStartPairLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

                stackLayoutTimeContent.Children.Add(TimeStartPairLabel);


                Label TimeEndPairLabel = new Label()
                {
                    Text = cellSchedule.TimesPair.TimeEnd.ToString("HH:mm"),
                    Padding = 0,
                    Margin =0
                };
                TimeEndPairLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

                stackLayoutTimeContent.Children.Add(TimeEndPairLabel);

                //------

                //Название предметов пары контейнер

                StackLayout stackLayoutAcademicSubjectsContent = new StackLayout()
                {
                    Spacing = 0,
                };
                Grid.SetColumn(stackLayoutAcademicSubjectsContent, 1);

                gridTopContainerCell.Children.Add(stackLayoutAcademicSubjectsContent);

                foreach (var academicSubject in cellSchedule.AcademicSubjects)
                {
                    Label AcademicSubjectPairLabel = new Label()
                    {
                        Text = academicSubject.Name,
                        Padding = 0,
                        Margin = 0
                    };
                    AcademicSubjectPairLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

                    stackLayoutAcademicSubjectsContent.Children.Add(AcademicSubjectPairLabel);
                }

                //------

                //Время пары контейнер

                StackLayout stackLayoutAudiencesContainer = new StackLayout()
                {
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Start,
                    HorizontalOptions =  LayoutOptions.Start
                };

                Grid.SetColumn(stackLayoutAudiencesContainer, 0);

                gridBottomContainerCell.Children.Add(stackLayoutAudiencesContainer);


                Image imagePinMap = new Image
                {
                    Source = ImageSource.FromResource("ScheduleMobileApp.Images.pin-map.png"),
                    WidthRequest = 15,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Start
                };

                stackLayoutAudiencesContainer.Children.Add(imagePinMap);


                StackLayout stackLayoutAudiencesContent = new StackLayout()
                {
                    Spacing = 0,
                };

                stackLayoutAudiencesContainer.Children.Add(stackLayoutAudiencesContent);

                foreach (var audience in cellSchedule.Audiences)
                {
                    Label AudiencePairLabel = new Label()
                    {
                        Text = audience.Name,
                        Padding = 0,
                        Margin = 0
                    };
                    AudiencePairLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

                    stackLayoutAudiencesContent.Children.Add(AudiencePairLabel);
                }

                //--------

                //Название предметов пары контейнер

                StackLayout stackLayoutTeachersContent = new StackLayout()
                {
                    Spacing = 0,
                };
                Grid.SetColumn(stackLayoutTeachersContent, 1);

                gridBottomContainerCell.Children.Add(stackLayoutTeachersContent);

                foreach (var teacher in cellSchedule.Teachers)
                {
                    Label TeacherPairLabel = new Label()
                    {
                        Text = teacher.NameInitials,
                        Padding = 0,
                        Margin = 0
                    };
                    TeacherPairLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

                    stackLayoutTeachersContent.Children.Add(TeacherPairLabel);
                }

                //------








                // название
                StackLayout NamesPairStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical
                };

                if (cellSchedule.AcademicSubjects != null)
                {
                    foreach (var academicSubject in cellSchedule.AcademicSubjects)
                    {
                        Label AcademicSubjectLabel = new Label()
                        {
                            Text = academicSubject.Name
                        };

                        AcademicSubjectLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

                        NamesPairStackLayout.Children.Add(AcademicSubjectLabel);
                    }

                }




                // Преподы
                StackLayout TeachersStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical
                };

                if (cellSchedule.Teachers != null)
                {
                    foreach (var teacher in cellSchedule.Teachers)
                    {
                        Label TeacherLabel = new Label()
                        {
                            Text = teacher.NameInitials
                        };
                        TeacherLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);


                        TeachersStackLayout.Children.Add(TeacherLabel);
                    }
                }

                // время и аудитория 
                // название и номер пары
                StackLayout AudincesPairStackLayout = new StackLayout()
                {
                    Orientation = StackOrientation.Vertical
                };


                if (cellSchedule.Audiences != null)
                {
                    foreach (var audience in cellSchedule.Audiences)
                    {
                        Label AudienceLabel = new Label()
                        {
                            Text = audience.Name
                        };
                        AudienceLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);


                        AudincesPairStackLayout.Children.Add(AudienceLabel);
                    }
                }

                StackLayout imagesStackLayout = new StackLayout()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center
                };

                Grid.SetColumnSpan(imagesStackLayout, 2);

                if (cellSchedule.IsChange)
                {
                    Image image = new Image { Source = ImageSource.FromResource("ScheduleMobileApp.Images.clock24.png"), WidthRequest = 25, HeightRequest = 25 };

                    imagesStackLayout.Children.Add(image);
                }

                /*gridContainerContent.Children.Add(imagesStackLayout);
                gridContainerContent.Children.Add(AudincesPairStackLayout, 1, 1);
                gridContainerContent.Children.Add(NamesPairStackLayout, 0, 1);
                gridContainerContent.Children.Add(TeachersStackLayout, 0, 3);*/

            }
            catch(Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);

            }

            return frameMainContainer;
        }
        

        private static Frame GetCellScheduleAdditionalInformationFrame(TimesPair timesPair, string  title)
        {
            Frame frameMainContainer = new Frame()
            {
                CornerRadius = 10,
                Padding = 0,
                Margin = 0,
                HasShadow = true,
            };
            frameMainContainer.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeDark"]);


            Grid gridMainContainer = new Grid()
            {
                RowDefinitions =
                    {
                        new RowDefinition() { Height = new GridLength(30, GridUnitType.Absolute) },
                        new RowDefinition()
                    },
                RowSpacing = 0
            };

            frameMainContainer.Content = gridMainContainer;


            Label TimesPairLabel = new Label()
            {
                Text = timesPair.TimeStart.ToString("HH:mm") + " - " + timesPair.TimeEnd.ToString("HH:mm"),
                HorizontalTextAlignment =  TextAlignment.Center,
                VerticalTextAlignment= TextAlignment.Center,
                BackgroundColor = (Color)App.Current.Resources["PurpleDefault"]
            };
            TimesPairLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);


            Label nameLabel = new Label()
            {
                Text = title,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = 16,
                Padding = new Thickness(10, 5)
            };
            nameLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

            gridMainContainer.Children.Add(TimesPairLabel, 0, 0);
            gridMainContainer.Children.Add(nameLabel, 0, 1);



            return frameMainContainer;
        }

        internal static async Task<Group[]?> GetAllGroups()
        {
            Group[]? groups = null;
            try
            {
                string responseString = string.Empty;


                try
                {
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(Config.UrlWebApi + "groups");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent responseContent = response.Content;
                        responseString = await responseContent.ReadAsStringAsync();
                    }
                }
                catch(Exception error)
                {
                    Services.Background.Worker.ErrorNotify(error);
                }



                if (!string.IsNullOrEmpty(responseString))
                {
                    var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonGroups.Root>(JsonConvert.DeserializeObject(responseString).ToString());

                    groups = root.Groups.ToArray();
                }
            }
            catch (Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);
            }

            return groups;
        }

        internal static async Task<ScheduleCellAdditionalInformation[]?> GetScheduleCellsAdditionalInformationAsync()
        {
            ScheduleCellAdditionalInformation[]? scheduleCellsAdditionalInformation = null;
            try
            {
                string responseString = string.Empty;


                try
                {
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(Config.UrlWebApi + "ScheduleCellsAdditionalInformation");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent responseContent = response.Content;
                        responseString = await responseContent.ReadAsStringAsync();
                    }
                }
                catch (Exception error)
                {
                    Services.Background.Worker.ErrorNotify(error);
                }



                if (!string.IsNullOrEmpty(responseString))
                {
                    var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonScheduleCellsAdditionalInformation.Root>(JsonConvert.DeserializeObject(responseString).ToString());

                    if (root?.CellsScheduleAdditionalInformation != null)
                    {
                        scheduleCellsAdditionalInformation = root.CellsScheduleAdditionalInformation.ToArray();
                    }
                }
            }
            catch (Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);
            }

            return scheduleCellsAdditionalInformation;
        }

        internal static async Task<ScheduleMobileApp.Models.CellSchedule[]> GetScheduleByGroup(Group group, DateTime  date)
        {
            ScheduleMobileApp.Models.CellSchedule[] cellSchedule = null;

            try
            {
                if (group == null)
                    return null;


                string responseString = string.Empty;

                 

                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(Config.UrlWebApi + $"schedule?group_id={group.Id}&date={date.ToString("yyyy.MM.dd")}");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    responseString = await responseContent.ReadAsStringAsync();
                }



                if (!string.IsNullOrEmpty(responseString))
                {
                    var responseDeserializeString = JsonConvert.DeserializeObject(responseString).ToString();
                    var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonCellsSchedule.Root>(responseDeserializeString);

                    cellSchedule = root.CellsSchedule.ToArray();
                }

            }
            catch (Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);
            }

            return cellSchedule;
        }

        internal static async Task<CellSchedule[]> GetChangeScheduleByGroup(Group group, DateTime date)
        {
            ScheduleMobileApp.Models.CellSchedule[] cellSchedule = null;

            try
            {
                if (group == null)
                    return null;


                string responseString = string.Empty;



                HttpClient client = new HttpClient();
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(Config.UrlWebApi + $"schedule?group_id={group.Id}&date={date.ToString("yyyy.MM.dd")}&is_change=true");
                request.Method = HttpMethod.Get;
                request.Headers.Add("Accept", "application/json");

                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    HttpContent responseContent = response.Content;
                    responseString = await responseContent.ReadAsStringAsync();
                }



                if (!string.IsNullOrEmpty(responseString))
                {
                    var responseDeserializeString = JsonConvert.DeserializeObject(responseString).ToString();
                    var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonCellsSchedule.Root>(responseDeserializeString);

                    cellSchedule = root.CellsSchedule.ToArray();
                }

            }
            catch (Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);
            }

            return cellSchedule;
        }


        internal static Grid GetDayOfWeekAndDateTopMenuCellSchedule(DateTime currentDateForCellSchedule)
        {
            Grid dayOfWeekAndDateTopMenuGridContainer = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition()
                },
                Padding=0,
                Margin =0
            };

            if (currentDateForCellSchedule == DateTime.MinValue)
                return dayOfWeekAndDateTopMenuGridContainer;
            
            if (currentDateForCellSchedule == DateTime.MaxValue)
                return dayOfWeekAndDateTopMenuGridContainer;


            try
            {
                DateTime date = GetDateNextMonday(currentDateForCellSchedule);


                for (int i = 0; i < 6; i++)
                {
                    var tempDate = date.AddDays(i);

                    Frame dayOfWeekAndDateForMenuCellScheduleFrame = GetDayOfWeekAndDateForMenuCellScheduleFrame(tempDate);

                    var tapGestureRecognizer = new TapGestureRecognizer();

                    tapGestureRecognizer.Tapped += TapGestureRecognizerDayOfWeekForCellScheduleMainWindow_Tapped;
                    tapGestureRecognizer.CommandParameter = tempDate;

                    dayOfWeekAndDateForMenuCellScheduleFrame.GestureRecognizers.Add(tapGestureRecognizer);

                    Grid.SetColumn(dayOfWeekAndDateForMenuCellScheduleFrame, i);

                    dayOfWeekAndDateTopMenuGridContainer.Children.Add(dayOfWeekAndDateForMenuCellScheduleFrame);

                }
            }
            catch(Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);

            }

            return dayOfWeekAndDateTopMenuGridContainer;
        }

        private static void TapGestureRecognizerDayOfWeekForCellScheduleMainWindow_Tapped(object sender, EventArgs e)
        {
            var tappedEventArgs = (TappedEventArgs)e;
            var parameter = tappedEventArgs.Parameter;

            var dateSelect = (DateTime)parameter;

            SchedulePage.SelectDateForCellSchedule = dateSelect;
        }

        

        private static Frame GetDayOfWeekAndDateForMenuCellScheduleFrame(DateTime date)
        {
            Frame frameMainContainer = new Frame()
            {
                Padding = new Thickness(15, 10),
                Margin = 0,
                CornerRadius = 100,
                HorizontalOptions = LayoutOptions.Center
            };
            frameMainContainer.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeDark"]);


            StackLayout stackLayoutContent = new StackLayout()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 5
            };

            frameMainContainer.Content = stackLayoutContent;

            Label dayLabel = new Label()
            {
                Text = date.ToString("dd"),
                Padding = 0,
                Margin = 0,
            };

            dayLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

            Label dayOfWeekLabel = new Label()
            {
                Text = GetShortNameOfDayOfWeekRus(date.DayOfWeek),
                Padding = 0,
                Margin = 0,
            };

            dayOfWeekLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);


            stackLayoutContent.Children.Add(dayLabel);
            stackLayoutContent.Children.Add(dayOfWeekLabel);


            return frameMainContainer;
        }

        private static string GetShortNameOfDayOfWeekRus(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday: return "Пн";
                case DayOfWeek.Tuesday: return "Вт";
                case DayOfWeek.Wednesday: return "Ср";
                case DayOfWeek.Thursday: return "Чт";
                case DayOfWeek.Friday: return "Пт";
                case DayOfWeek.Saturday: return "Сб";
                case DayOfWeek.Sunday: return "Вс";
                default: return "";
            }
        }

        private static DateTime GetDateNextMonday(DateTime currentDateForCellSchedule)
        {
            int addDay = 0;

            switch (currentDateForCellSchedule.DayOfWeek)
            {
                case DayOfWeek.Monday: addDay = 0; break;
                case DayOfWeek.Tuesday: addDay = -1; break;
                case DayOfWeek.Wednesday: addDay = -2; break;
                case DayOfWeek.Thursday: addDay = -3; break;
                case DayOfWeek.Friday: addDay = -4; break;
                case DayOfWeek.Saturday: addDay = -5; break;
                case DayOfWeek.Sunday: addDay = 1; break;
            }

            DateTime date = currentDateForCellSchedule.AddDays(addDay);

            return date;
        }

        internal static async Task<Models.DateByNumeratorAndDenominator?> GetCellScheduleTypeAsync(DateTime selectDateForCellSchedule)
        {
            

            try
            {
                string responseString = string.Empty;


                try
                {
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(Config.UrlWebApi + $"CellScheduleType?date={selectDateForCellSchedule.ToString("yyyy.MM.dd")}");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent responseContent = response.Content;
                        responseString = await responseContent.ReadAsStringAsync();
                    }
                }
                catch (Exception error)
                {
                    Services.Background.Worker.ErrorNotify(error);
                }



                if (!string.IsNullOrEmpty(responseString))
                {
                    var jsonDateByNumeratorAndDenominator = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonDateByNumeratorAndDenominator>(JsonConvert.DeserializeObject(responseString).ToString());

                    if (jsonDateByNumeratorAndDenominator?.DateByNumeratorAndDenominator != null)
                    {
                        return jsonDateByNumeratorAndDenominator.DateByNumeratorAndDenominator;
                    }
                }
            }
            catch (Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);
            }

            return null;
        }

        internal static string GetStringForTypeOfWeek(DateByNumeratorAndDenominator? cellScheduleType)
        {
            string result = "Нет данных";

            if (cellScheduleType == null)
                return result;

            switch (cellScheduleType.TypeCellSchedule)
            {
                case CellScheduleType.common: result = "Общий"; break;
                case CellScheduleType.numerator: result = "Числитель"; break;
                case CellScheduleType.denominator: result = "Знаменатель"; break;
            }

            return result;
        }

        internal static Frame GetCellMenuMini(string nameIcon, string nameMenu, TapGestureRecognizer tapGestureRecognizer)
        {

            StackLayout stackLayoutContainer = new StackLayout()
            {
                Padding =0,
                Margin=0
            };

            Image imageIconMenu = new Image
            {
                
                WidthRequest=50,
                Source = ImageSource.FromResource($"ScheduleMobileApp.Images.{nameIcon}"),
                HorizontalOptions = LayoutOptions.Center,
            };

            Label nameMenuLabel = new Label()
            {
                Padding = 0,
                Margin = 0,
                Text = nameMenu,
                HorizontalTextAlignment = TextAlignment.Center
            };

            nameMenuLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);


            stackLayoutContainer.Children.Add(imageIconMenu);
            stackLayoutContainer.Children.Add(nameMenuLabel);



            Frame frame = new Frame()
            {
                Padding = 10,
                Content = stackLayoutContainer,
                CornerRadius =  10,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                BackgroundColor = Color.Red
            };

            frame.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeDark"]);


            frame.GestureRecognizers.Add(tapGestureRecognizer);


            return frame;
        }

        internal static async Task<TimesPair[]?> GetTimesPairsAsync(DayOfWeek dayOfWeek)
        {
            try
            {
                string responseString = string.Empty;


                try
                {
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(Config.UrlWebApi + $"TimesPairs?DayOfWeek={dayOfWeek}");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent responseContent = response.Content;
                        responseString = await responseContent.ReadAsStringAsync();
                    }
                }
                catch (Exception error)
                {
                    Services.Background.Worker.ErrorNotify(error);
                }



                if (!string.IsNullOrEmpty(responseString))
                {
                    var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonTimesPairs.Root>(JsonConvert.DeserializeObject(responseString).ToString());

                    if (root != null)
                    {
                        if (root.TimesPairs != null)
                        {
                            return root.TimesPairs.ToArray(); 
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);
            }

            return null;
        }

		internal static async Task<CellScheduleExam[]?> GetCellsScheduleExamsForGroupAsync(int groupId)
		{
			try
			{
				string responseString = string.Empty;


				try
				{
					HttpClient client = new HttpClient();
					HttpRequestMessage request = new HttpRequestMessage();
					request.RequestUri = new Uri(Config.UrlWebApi + $"CellsScheduleExams?group_id={groupId}");
					request.Method = HttpMethod.Get;
					request.Headers.Add("Accept", "application/json");

					HttpResponseMessage response = await client.SendAsync(request);
					if (response.StatusCode == HttpStatusCode.OK)
					{
						HttpContent responseContent = response.Content;
						responseString = await responseContent.ReadAsStringAsync();
					}
				}
				catch (Exception error)
				{
					Services.Background.Worker.ErrorNotify(error);
				}



				if (!string.IsNullOrEmpty(responseString))
				{
					var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonCellsScheduleExams.Root>(JsonConvert.DeserializeObject(responseString).ToString());

					if (root != null)
					{
						if (root.CellsScheduleExams != null)
						{
							return root.CellsScheduleExams.ToArray();
						}
					}
				}
			}
			catch (Exception error)
			{
				Services.Background.Worker.ErrorNotify(error);
			}

			return null;
		}
		internal static async Task<MobileApp[]?> GetMobileAppsAsync()
		{
			try
			{
				string responseString = string.Empty;


				try
				{
					HttpClient client = new HttpClient();
					HttpRequestMessage request = new HttpRequestMessage();
					request.RequestUri = new Uri(Config.UrlWebApi + $"MobileApps");
					request.Method = HttpMethod.Get;
					request.Headers.Add("Accept", "application/json");

					HttpResponseMessage response = await client.SendAsync(request);
					if (response.StatusCode == HttpStatusCode.OK)
					{
						HttpContent responseContent = response.Content;
						responseString = await responseContent.ReadAsStringAsync();
					}
				}
				catch (Exception error)
				{
					Services.Background.Worker.ErrorNotify(error);
				}



				if (!string.IsNullOrEmpty(responseString))
				{
					var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonMobileApps.Root>(JsonConvert.DeserializeObject(responseString).ToString());

					if (root != null)
					{
						if (root.MobileApps != null)
						{
							return root.MobileApps;
						}
					}
				}
			}
			catch (Exception error)
			{
				Services.Background.Worker.ErrorNotify(error);
			}

			return null;
		}

		internal static Frame GetFrameContainerTimesPair(TimesPair timesPair, TimesPair? oldTimesPair = null)
        {
            Frame frameContainer = new Frame()
            {
                Padding= 0,
                CornerRadius = 10,
                MinimumHeightRequest = 50,
                HeightRequest = 50
            };

            if (timesPair.IsChange)
            {
                frameContainer.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorChangeThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorChangeThemeDark"]);
            }
            else
            {
                frameContainer.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeDark"]);
            }


            Grid gridConatiner = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition(){ Width = new GridLength(30, GridUnitType.Absolute)},
                    new ColumnDefinition(){ Width = new GridLength(1,  GridUnitType.Star)},
                    new ColumnDefinition(){ Width = new GridLength(1,  GridUnitType.Star)},
                    new ColumnDefinition(){ Width = new GridLength(1,  GridUnitType.Star)}
                },
            };

            frameContainer.Content = gridConatiner;

            Frame frameContainerNumberPair = new Frame()
            {
                BackgroundColor = (Color)App.Current.Resources["PurpleDefault"],
                Padding =0,
                CornerRadius =0
            };

            Label numberPairLabel = new Label()
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Text = timesPair.NumberPair.ToString(),
                TextColor = (Color)App.Current.Resources["ColorTextDefaultDark"],
                Padding = 0
            };

            //numberPairLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

            frameContainerNumberPair.Content = numberPairLabel;

            Grid.SetColumn(frameContainerNumberPair, 0);
            gridConatiner.Children.Add(frameContainerNumberPair);


            Label timeStartLabel = new Label()
            {
                Padding = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Text = timesPair.TimeStart.ToString("HH:mm")
            };

            timeStartLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

            Grid.SetColumn(timeStartLabel, 1);
            gridConatiner.Children.Add(timeStartLabel);

            

            Label timeLineLabel = new Label()
            {
                Padding = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Text = "-"
            };
            timeLineLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);
            Grid.SetColumn(timeLineLabel, 2);
            gridConatiner.Children.Add(timeLineLabel);



            Label timeEndLabel = new Label()
            {
                Padding = 5,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Text = timesPair.TimeEnd.ToString("HH:mm")
            };
            timeEndLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);
            Grid.SetColumn(timeEndLabel, 3);
            gridConatiner.Children.Add(timeEndLabel);




            return frameContainer;
        }

        internal static async Task<DateByNumeratorAndDenominator[]> GetDatesNumeratorAndDenominatorAsync()
        {
            try
            {
                string responseString = string.Empty;


                try
                {
                    HttpClient client = new HttpClient();
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(Config.UrlWebApi + $"DatesNumeratorAndDenominator");
                    request.Method = HttpMethod.Get;
                    request.Headers.Add("Accept", "application/json");

                    HttpResponseMessage response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        HttpContent responseContent = response.Content;
                        responseString = await responseContent.ReadAsStringAsync();
                    }
                }
                catch (Exception error)
                {
                    Services.Background.Worker.ErrorNotify(error);
                }



                if (!string.IsNullOrEmpty(responseString))
                {
                    var root = JsonConvert.DeserializeObject<ScheduleMobileApp.Models.Json.JsonDatesByNumeratorAndDenominator>(JsonConvert.DeserializeObject(responseString).ToString());

                    if (root != null)
                    {
                        if (root.DatesNumeratorAndDenominator != null)
                        {
                            return root.DatesNumeratorAndDenominator.ToArray();
                        }
                    }
                }
            }
            catch (Exception error)
            {
                Services.Background.Worker.ErrorNotify(error);
            }

            return null;
        }

        internal static Frame GetFrameContainerDateByNumeratorAndDenominator(DateByNumeratorAndDenominator dateByNumeratorAndDenominator)
        {
            Frame frameContainer = new Frame()
            {
                Padding =0,
                CornerRadius = 10
            };

            

            var dateStart = DateTime.Parse(dateByNumeratorAndDenominator.DateStart.ToString("dd.MM.yyyy"));
            var dateEnd = DateTime.Parse(dateByNumeratorAndDenominator.DateEnd.ToString("dd.MM.yyyy"));

            var dateNow = DateTime.Now;

            if(dateStart <= dateNow && dateNow < dateEnd)
            {
                frameContainer.SetAppThemeColor(Frame.BorderColorProperty, (Color)App.Current.Resources["ContentLevel1BorderColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BorderColorThemeDark"]);  
            }

            frameContainer.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeDark"]);

            Grid gridContainer = new Grid();

            frameContainer.Content = gridContainer;


            LinearGradientBrush linearGradientBrushProgress = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 1),
                EndPoint = new Point(1, 1),
                GradientStops =
                {
                     new GradientStop(){ Color = (Color)App.Current.Resources["PurpleDefault"], Offset = 0 },
                     new GradientStop(){ Color = Color.Transparent, Offset = 1 },
                }
            };

            float progressPercent = ((float)Services.Background.Worker.GetProgressDateNumeratorAndDenominator(dateByNumeratorAndDenominator)) / 100;

            linearGradientBrushProgress.GradientStops.Insert(1, new GradientStop() { Color = (Color)App.Current.Resources["PurpleDefault"], Offset = progressPercent });
            linearGradientBrushProgress.GradientStops.Insert(2, new GradientStop() { Color = Color.Transparent, Offset = progressPercent });




            

            BoxView boxViewGradientProgress = new BoxView()
            {
                Background = linearGradientBrushProgress
            };

            gridContainer.Children.Add(boxViewGradientProgress);



            Grid gridContentInfo = new Grid()
            { 
                Margin = 10,
                ColumnDefinitions =
                {
                    new ColumnDefinition(),
                    new ColumnDefinition(),
                    new ColumnDefinition()
                }
            };

            gridContainer.Children.Add(gridContentInfo);






            Label dateStartLabel = new Label()
            {
                Text = dateByNumeratorAndDenominator.DateStart.ToString("dd.MM.yyyy"),
                Padding = 0,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            dateStartLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

            Grid.SetColumn(dateStartLabel, 0);

            gridContentInfo.Children.Add(dateStartLabel);


            Label typeOfWeekLabel = new Label()
            {
                Text = Services.Background.Worker.GetStringForTypeOfWeek(dateByNumeratorAndDenominator),
                Padding = 0,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            typeOfWeekLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

            Grid.SetColumn(typeOfWeekLabel, 1);

            gridContentInfo.Children.Add(typeOfWeekLabel);



            Label dateEndLabel = new Label()
            {
                Text = dateByNumeratorAndDenominator.DateEnd.ToString("dd.MM.yyyy"),
                Padding = 0,
                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.Center
            };
            dateEndLabel.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

            Grid.SetColumn(dateEndLabel, 2);

            gridContentInfo.Children.Add(dateEndLabel);


            return frameContainer;
        }

        private static int GetProgressDateNumeratorAndDenominator(Models.DateByNumeratorAndDenominator dateByNumeratorAndDenominator)
        {
            var dateNowRound = DateTime.Parse(DateTime.Now.ToString("dd.MM.yyyy") + " 00:00:00");


            double result = 0;




            if (dateByNumeratorAndDenominator.DateEnd <= dateNowRound)
            {
                result = 100;
            }
            else if (dateByNumeratorAndDenominator.DateStart >= dateNowRound)
            {
                result = 0;
            }
            else
            {
                double totalDays = (dateByNumeratorAndDenominator.DateEnd - dateByNumeratorAndDenominator.DateStart).Days;
                double currentDay = (dateNowRound - dateByNumeratorAndDenominator.DateStart).Days;

                result = (currentDay / totalDays) * 100;
            }

            return Convert.ToInt32(result);
        }

		internal static Frame GetFrameContainerCellScheduleExam(CellScheduleExam cellScheduleExam)
		{
            Frame frameContainer = new Frame()
            {
                CornerRadius = 10,
                Padding = 0,
                Margin = 0
            };

			frameContainer.SetAppThemeColor(Frame.BackgroundColorProperty, (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeLight"], (Color)App.Current.Resources["ContentLevel1BackgroundColorThemeDark"]);

            StackLayout stackLayoutContainer = new StackLayout()
            {
                Padding = new Thickness(0, 10)
            };

            frameContainer.Content = stackLayoutContainer;


			Label labelTypeExamStatus = new Label()
			{
				Padding = 0,
				Margin = 0,
				Text = Services.Background.Worker.GetCellScheduleExamTypeStatusDate(cellScheduleExam.Date),
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			labelTypeExamStatus.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

			stackLayoutContainer.Children.Add(labelTypeExamStatus);


			Label labelCellScheduleExamType = new Label()
            {
				Padding = 0,
				Margin = 0,
				Text = Services.Background.Worker.GetCellScheduleExamType(cellScheduleExam.CellScheduleExamType),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
			};

			labelCellScheduleExamType.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

			stackLayoutContainer.Children.Add(labelCellScheduleExamType);



            Grid gridContainer = new Grid()
            {
                ColumnDefinitions =
                {
                    new ColumnDefinition() {Width = new GridLength(80, GridUnitType.Absolute)},
                    new ColumnDefinition(),
                },
                Padding = 10
            };

            stackLayoutContainer.Children.Add(gridContainer);


            StackLayout stackLayoutDateInfo = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };

			gridContainer.Children.Add(stackLayoutDateInfo);



			Label labelDate = new Label()
			{
				Text = cellScheduleExam.Date.ToString("dd.MM"),
                Padding =0,
                Margin =0,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			labelDate.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

			stackLayoutDateInfo.Children.Add(labelDate);

			Label labelDayOfWeekRus = new Label()
			{
				Text = GetDayOfWeekRus(cellScheduleExam.Date.DayOfWeek, abbreviated:false),
				Padding = 0,
				Margin = 0,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			labelDayOfWeekRus.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

			stackLayoutDateInfo.Children.Add(labelDayOfWeekRus);




			StackLayout stackLayoutInfoTitleAndTeacherAndAudience = new StackLayout()
			{
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center
			};

			gridContainer.Children.Add(stackLayoutInfoTitleAndTeacherAndAudience, 1, 0);


			Label labelTitle = new Label()
			{
				Text = cellScheduleExam.Title,
				Padding = 0,
				Margin = 0,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center
			};

			labelTitle.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

			stackLayoutInfoTitleAndTeacherAndAudience.Children.Add(labelTitle);


			Label labelTeacher = new Label()
			{
				Text = cellScheduleExam.Teacher.NameInitials,
				Padding = 0,
				Margin = 0,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center
			};

			labelTeacher.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

			stackLayoutInfoTitleAndTeacherAndAudience.Children.Add(labelTeacher);


			Label labelAudience= new Label()
			{
				Text = cellScheduleExam.Audience.Name,
				Padding = 0,
				Margin = 0,
				HorizontalOptions = LayoutOptions.Start,
				VerticalOptions = LayoutOptions.Center
			};

			labelAudience.SetAppThemeColor(Label.TextColorProperty, (Color)App.Current.Resources["ColorTextDefaultLight"], (Color)App.Current.Resources["ColorTextDefaultDark"]);

			stackLayoutInfoTitleAndTeacherAndAudience.Children.Add(labelAudience);


			return frameContainer;
		}

		public static string GetDayOfWeekRus(System.DayOfWeek dayOfWeek, bool abbreviated)
		{
			if (abbreviated)
			{
				switch (dayOfWeek)
				{
					case DayOfWeek.Monday: return "Пн";
					case DayOfWeek.Tuesday: return "Вт";
					case DayOfWeek.Wednesday: return "Ср";
					case DayOfWeek.Thursday: return "Чт";
					case DayOfWeek.Friday: return "Пт";
					case DayOfWeek.Saturday: return "Сб";
					case DayOfWeek.Sunday: return "Вс";
					default: return "";
				}
			}
			else
			{
				switch (dayOfWeek)
				{
					case DayOfWeek.Monday: return "Понедельник";
					case DayOfWeek.Tuesday: return "Вторник";
					case DayOfWeek.Wednesday: return "Среда";
					case DayOfWeek.Thursday: return "Четверг";
					case DayOfWeek.Friday: return "Пятница";
					case DayOfWeek.Saturday: return "Суббота";
					case DayOfWeek.Sunday: return "Воскресенье";
					default: return "";
				}
			}
		}

		private static string GetCellScheduleExamTypeStatusDate(DateTime date)
		{
            var dateNow = DateTime.Now;

			if (date > DateTime.Now)
            {
                return $"Осталось {(date - dateNow).Days} дней";
            }
            else
            {
				return $"Прошло";
			}
		}

		public static string GetCellScheduleExamType(Types.Enums.CellScheduleExamType cellScheduleExamType)
		{
			switch (cellScheduleExamType)
			{
				case Types.Enums.CellScheduleExamType.Сonsultation: return "Консультация";
				case Types.Enums.CellScheduleExamType.Exam: return "Экзамен";
				default: return "Нет данных";
			}
		}

		
	}
}
