using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using POF.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using static POF.ViewModels.DaySelectViewModel;

namespace POF.ViewModels
{
    //public class DisplayAlarm : ViewModelBase
    //{
    //    private string _alarmName;
    //    private string _daysSelected;
    //    private string _timeSelected;
    //    private bool _isAlarmOn;

    //    public int id { get; set; }

    //    public string AlarmName
    //    {
    //        get { return _alarmName; }
    //        set { _alarmName = value; OnPropertyChanged(); }
    //    }

    //    public string DaysSelected
    //    {
    //        get { return _daysSelected; }
    //        set { _daysSelected = value; OnPropertyChanged(); }
    //    }

    //    public bool IsAlarmOn
    //    {
    //        get { return _isAlarmOn; }
    //        set { _isAlarmOn = value; OnPropertyChanged(); }
    //    }

    //    public string TimeSelected
    //    {
    //        get { return _timeSelected; }
    //        set { _timeSelected = value; OnPropertyChanged(); }
    //    }

    //    private SoundData selectedSong;
    //    public SoundData SelectedSong
    //    {
    //        get { return selectedSong; }
    //        set { selectedSong = value;OnPropertyChanged(); }
    //    }
    //}


    public class MainPageViewModel : ViewModelBase
    {
        private ObservableCollection<AlarmEvent> _savedAlarmCollection;
        public ObservableCollection<AlarmEvent> SavedAlarmCollection
        {
            get { return _savedAlarmCollection; }
            set { _savedAlarmCollection = value; OnPropertyChanged(); }
        }

        public RelayCommand AddNewAlarmCommand { get; set; }

        public string AudioName { get; set; }




        private void InvokeToast()
        {
            ToastNotificationManager.History.Clear();

            setToast("MorningAlarm", "10:38 PM", Path, "10");
        }




        private void setToast(string alarmName, string alarmTime, string soundPath, string snoozeTime)
        {
            string xml = $@"<toast activationType='foreground' scenario='reminder' launch='args'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                <image placement='AppLogoOverride' src='Assets/Alarm_Icon.png'/>
                                                <text>Alarm</text>
                                                <text>{alarmName}</text>
                                                <text>{alarmTime}</text>
                                                </binding>
                                            </visual>
                    <actions>
                       <input id='snoozeTime' type='selection' defaultInput='{snoozeTime}'>
                            <selection id='5' content  = '5 minutes'/>
                            <selection id='10' content = '10 minutes'/>
                            <selection id='20' content = '20 minutes'/>
                            <selection id='30' content = '30 minutes'/>
                            <selection id='60' content = '1 hour'/>
                        </input>
                    <action activationType='system' arguments='snooze' hint-inputId='snoozeTime' content='' />
                    <action activationType='system' arguments='dismiss' content='' />
                    </actions>
                <audio src='{soundPath}' loop='true'/>
                                        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            // var toast = new ToastNotification(doc);
            // ToastNotificationManager.CreateToastNotifier().Show(toast);

            //double snoozeMin;
            //TimeSpan snoozeTimeSpan=TimeSpan.FromMinutes(10);
            //bool canSnooze = Double.TryParse(snoozeTime, out snoozeMin);
            //if(canSnooze)
            //{
            //    snoozeTimeSpan = TimeSpan.FromMinutes(snoozeMin);
            //}

            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();

            DateTime sceduleTime = DateTime.Now.AddMinutes(1);

            var sceduleToast = new ScheduledToastNotification(doc, sceduleTime);
            toastNotifier.AddToSchedule(sceduleToast);
        }

        //TODO: add checking for blank spaces
        public string Path { get; set; }

        private INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
        {
            //Messenger.Default.Register<SoundData>(
            //    this,
            //    sound =>
            //{
            //    Path = sound.ToastFilePath;
            //});
            _navigationService = navigationService;
            AddNewAlarmCommand = new RelayCommand(navigateToAddPage);
            getNewAlarms();
            SavedAlarmCollection = new ObservableCollection<AlarmEvent>();


            SetAlarmList = new List<NextAlarm>();
        }

        private void navigateToAddPage()
        {
            _navigationService.NavigateTo("AddPage");
        }


        private void getNewAlarms()
        {
            Messenger.Default.Register<AlarmEvent>(
                this,
                alarm =>
                {
                    SavedAlarmCollection.Add(new AlarmEvent
                    {
                        AlarmName = alarm.AlarmName,
                        SelectedDays=alarm.SelectedDays,
                        IsAlarmOn = alarm.IsAlarmOn,                   
                        SelectedSound = alarm.SelectedSound,
                        TimeSet = alarm.TimeSet,
                        SnoozeTime=alarm.SnoozeTime
                        
                    });

                    createAlarms(alarm);
                    //setToast(); 
                });
        }


        public class NextAlarm
        {
            public string AlarmName { get; set; }
            public string SongPath { get; set; }
            public DateTime Day { get; set; }
        }


        public List<NextAlarm> SetAlarmList { get; set; }

        public NextAlarm createAlarms(AlarmEvent alarmEvent)
        {
            var alarm = new NextAlarm();


            SelectableDay selected = (SelectableDay)alarmEvent.SelectedDays.DisplayNameNum;


            Calendar calendar = CultureInfo.CurrentCulture.Calendar;

            DateTime date = DateTime.Today;


            foreach (SelectableDay selectedDay in Enum.GetValues(typeof(SelectableDay)))
            {
                if (selected.HasFlag(selectedDay))
                {
                    DayOfWeek dayNeeded = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(SelectableDay), selectedDay));

                    while (date.DayOfWeek != dayNeeded)
                    {
                        date = date.AddDays(1);
                    }

                    var DaySelected = new DateTime(date.Year, date.Month, date.Day, alarmEvent.TimeSet.Hours, alarmEvent.TimeSet.Minutes, alarmEvent.TimeSet.Seconds);

                    SetAlarmList.Add(new NextAlarm { AlarmName = alarmEvent.AlarmName, SongPath = alarmEvent.SelectedSound.FilePath, Day = DaySelected });
                }
            }

            return alarm;
        }



        private NextAlarm returnNextAlarm()
        {
            var nextAlarm = SetAlarmList.OrderBy(x => x.Day).FirstOrDefault();

            return nextAlarm;
        }



    }
}