using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using POF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using static POF.ViewModels.DaySelectViewModel;

namespace POF.ViewModels
{

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


        //private void InvokeToast()
        //{
        //    ToastNotificationManager.History.Clear();

        //    setToast("MorningAlarm", "10:38 PM", Path, "10");
        //}



        //private void setToast(string alarmName, string alarmTime, string soundPath, string snoozeTime)
        //{
        //    string xml = $@"<toast activationType='foreground' scenario='reminder' launch='args'>
        //                                    <visual>
        //                                        <binding template='ToastGeneric'>
        //                                        <image placement='AppLogoOverride' src='Assets/Alarm_Icon.png'/>
        //                                        <text>Alarm</text>
        //                                        <text>{alarmName}</text>
        //                                        <text>{alarmTime}</text>
        //                                        </binding>
        //                                    </visual>
        //            <actions>
        //               <input id='snoozeTime' type='selection' defaultInput='{snoozeTime}'>
        //                    <selection id='5' content  = '5 minutes'/>
        //                    <selection id='10' content = '10 minutes'/>
        //                    <selection id='20' content = '20 minutes'/>
        //                    <selection id='30' content = '30 minutes'/>
        //                    <selection id='60' content = '1 hour'/>
        //                </input>
        //            <action activationType='system' arguments='snooze' hint-inputId='snoozeTime' content='' />
        //            <action activationType='system' arguments='dismiss' content='' />
        //            </actions>
        //        <audio src='{soundPath}' loop='true'/>
        //                                </toast>";

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(xml);

        //    // var toast = new ToastNotification(doc);
        //    // ToastNotificationManager.CreateToastNotifier().Show(toast);

        //    //double snoozeMin;
        //    //TimeSpan snoozeTimeSpan=TimeSpan.FromMinutes(10);
        //    //bool canSnooze = Double.TryParse(snoozeTime, out snoozeMin);
        //    //if(canSnooze)
        //    //{
        //    //    snoozeTimeSpan = TimeSpan.FromMinutes(snoozeMin);
        //    //}

        //    ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();

        //    DateTime sceduleTime = DateTime.Now.AddMinutes(1);

        //    var sceduleToast = new ScheduledToastNotification(doc, sceduleTime);
        //    toastNotifier.AddToSchedule(sceduleToast);
        //}


     /// <summary>
     /// Set alarm toast notification
     /// </summary>
     /// <param name="nextAlarm"></param>
        private void setToast(NextAlarm nextAlarm)
        {
            //ToastNotificationManager.History.Clear();


            string alarmName = nextAlarm.AlarmName;
            string alarmTime = nextAlarm.Day.ToString("hh:mm tt");

            string snoozeTime = nextAlarm.SnoozeTime;
            string soundPath = nextAlarm.ToastSongPath;
        

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

            //var toast = new ToastNotification(doc);
            //ToastNotificationManager.CreateToastNotifier().Show(toast);

            //double snoozeMin;
            //TimeSpan snoozeTimeSpan=TimeSpan.FromMinutes(10);
            //bool canSnooze = Double.TryParse(snoozeTime, out snoozeMin);
            //if(canSnooze)
            //{
            //    snoozeTimeSpan = TimeSpan.FromMinutes(snoozeMin);
            //}


            DateTime sceduleTime = nextAlarm.Day;


            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();


            var toast = new ScheduledToastNotification(doc, sceduleTime);

           
            toast.Id = nextAlarm.ID;


            toastNotifier.AddToSchedule(toast);

            setNextAlarm();
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


            NewAlarmList = new List<NextAlarm>();
        }

        private void navigateToAddPage()
        {
            _navigationService.NavigateTo("AddPage");
        }


        /// <summary>
        /// Get new instance of alarm from the AlarmPageViewModel
        /// </summary>
        private void getNewAlarms()
        {
            Messenger.Default.Register<AlarmEvent>(
                this,
                ReceivedAlarm =>
                {
                    SavedAlarmCollection.Add(new AlarmEvent
                    {
                        AlarmName = ReceivedAlarm.AlarmName,
                        SelectedDays=ReceivedAlarm.SelectedDays,
                        IsAlarmOn = ReceivedAlarm.IsAlarmOn,                   
                        SelectedSound = ReceivedAlarm.SelectedSound,
                        TimeSet = ReceivedAlarm.TimeSet,
                        SnoozeTime=ReceivedAlarm.SnoozeTime
                        
                    });
                    createNextAlarm(new DateTime(2016,07,14,13,30,0), ReceivedAlarm);
                });
        }


        public class NextAlarm
        {
            public string ID => Guid.NewGuid().ToString().Split('-').First();



            public string AlarmName { get; set; }
            public string ToastSongPath { get; set; }
            public DateTime Day { get; set; }
            public string SnoozeTime { get; set; }
        }


        public List<NextAlarm> NewAlarmList { get; set; }
        DateTime dateToday;

        /// <summary>
        /// Check if same day as today was selected and if its too late for today, transfer alarm for the next day
        /// </summary>
        /// <param name="alarmEvent"></param>
        /// <param name="selectedDay"></param>
        /// <returns></returns>
        private bool isSameDay(AlarmEvent alarmEvent, DayOfWeek selectedDay,DateTime FromDateTime)
        {
            bool isTimeAhead = false;
            var timeSelected = Convert.ToDateTime(alarmEvent.TimeSet.ToString());
          
            if (selectedDay== FromDateTime.DayOfWeek)
            {
                isTimeAhead=timeSelected < DateTime.Now;
            } 
            return isTimeAhead;
        }

        /// <summary>
        /// Create new alarm instance of alarm from selected days and time and add it to the NewAlarmList
        /// </summary>
        /// <param name="alarmEvent"></param>
        //public void createNextAlarm(AlarmEvent alarmEvent)
        //{
        //    SelectableDay selected = (SelectableDay)alarmEvent.SelectedDays.DisplayNameNum;

        //    bool isDayTodaySelected;

        //    if (selected != 0)
        //    {
        //        foreach (SelectableDay selectedDay in Enum.GetValues(typeof(SelectableDay)))
        //        {
        //            dateToday = DateTime.Today;

        //            if (selected.HasFlag(selectedDay))
        //            {
        //                DayOfWeek dayHasFlag= (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(SelectableDay), selectedDay));

        //                isDayTodaySelected = isSameDay(alarmEvent, dayHasFlag);

        //                while (dateToday.DayOfWeek != dayHasFlag||isDayTodaySelected)
        //                {
        //                    dateToday = dateToday.AddDays(1);
        //                    isDayTodaySelected = false;
        //                }

        //                addAlarmToList(dateToday, alarmEvent);
        //            }
        //        }
        //    }

        //    //if day repeat selected "only once"
        //    else
        //    {
        //        dateToday = Convert.ToDateTime(alarmEvent.TimeSet.ToString());

        //        if(dateToday.TimeOfDay<DateTime.Now.TimeOfDay)
        //        {
        //            dateToday = dateToday.AddDays(1);
        //        }

        //        addAlarmToList(dateToday, alarmEvent);
        //    }

        //    setNextAlarm();
        //}

        private bool isDayTodaySelected;

        public void createNextAlarm(DateTime FromDateTime, AlarmEvent alarmEvent)
        {
            SelectableDay selected = (SelectableDay)alarmEvent.SelectedDays.SelectableDayInt;
            
            if (selected != 0)
            {
                foreach (SelectableDay selectedDay in Enum.GetValues(typeof(SelectableDay)))
                {
                    if (selected.HasFlag(selectedDay))
                    {
                        // DayOfWeek dayHasFlag = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(SelectableDay), selectedDay));

                        DateTime  dayHasFlag = (DateTime)Enum.Parse(typeof(DateTime), Enum.GetName(typeof(SelectableDay), selectedDay));


                        //isDayTodaySelected = isSameDay(alarmEvent, dayHasFlag);

                        // while (FromDateTime.DayOfWeek != dayHasFlag || isSameDay(alarmEvent, dayHasFlag)==true)


                      //  var test = new DateTime(dayHasFlag.)

                         while (FromDateTime.DayOfWeek != dayHasFlag.DayOfWeek)
                        {
                            FromDateTime = FromDateTime.AddDays(1);
                           // isDayTodaySelected = false;
                        }

                        addAlarmToList(FromDateTime, alarmEvent);
                    }
                }
            }

            //if day repeat selected "only once"
            else
            {
               var selectedDateTime = Convert.ToDateTime(alarmEvent.TimeSet.ToString());

                if (FromDateTime.TimeOfDay > selectedDateTime.TimeOfDay)
                {
                    FromDateTime = FromDateTime.AddDays(1);
                }

                addAlarmToList(FromDateTime, alarmEvent);
            }

            setNextAlarm();
        }








        /// <summary>
        /// Add alarm to the NewAlarmList 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="alarmEvent"></param>
        private void addAlarmToList(DateTime dateTime,AlarmEvent alarmEvent)
        {
            var DayTimeSelected = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, alarmEvent.TimeSet.Hours, alarmEvent.TimeSet.Minutes, alarmEvent.TimeSet.Seconds);
            NewAlarmList.Add(new NextAlarm { AlarmName = alarmEvent.AlarmName, ToastSongPath = alarmEvent.SelectedSound.ToastFilePath, Day = DayTimeSelected, SnoozeTime = alarmEvent.SnoozeTime.SnoozeMin });
            
        }

        /// <summary>
        /// Select what alarm comes first and set the toast
        /// </summary>
        private void setNextAlarm()
        {
            //Check previous alarm: is still on(move to next week), off, or remove from list


            NextAlarm alarm = NewAlarmList.OrderBy(x => x.Day).FirstOrDefault();
            
             setToast(alarm);
           
        }
        
    }
}