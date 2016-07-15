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
        private INavigationService _navigationService;

        public ObservableCollection<AlarmEvent> SavedAlarmCollection
        {
            get { return _savedAlarmCollection; }
            set { _savedAlarmCollection = value; OnPropertyChanged(); }
        }

        public RelayCommand AddNewAlarmCommand { get; set; }

        /// <summary>
        /// Set alarm toast notification
        /// </summary>
        /// <param name="nextAlarm"></param>
        private void setToast(AlarmEvent nextAlarm)
        {
            ToastNotificationManager.History.Clear();

            string alarmName = nextAlarm.AlarmName;
            string alarmTime = nextAlarm.Day.ToString("hh:mm tt");

            string snoozeTime = nextAlarm.SnoozeTime.SnoozeMin;
            string soundPath = nextAlarm.SelectedSound.ToastFilePath;

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

            DateTime sceduleTime = nextAlarm.Day;

            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier();

            var toast = new ScheduledToastNotification(doc, sceduleTime);

            toast.Id = nextAlarm.ID;

            toastNotifier.AddToSchedule(toast);
        }



        private List<AlarmEvent> _nextAlarmList;

        public MainPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            AddNewAlarmCommand = new RelayCommand(navigateToAddPage);
            SavedAlarmCollection = new ObservableCollection<AlarmEvent>();

            _nextAlarmList = new List<AlarmEvent>();
            getNewAlarms();
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
                alarm =>
                {
                    SavedAlarmCollection.Add(alarm);
                    createNextAlarm(new DateTime(2016, 07, 15, 15, 00, 0), alarm);
                });
        }



        private void createNextAlarm(DateTime FromDateTime, AlarmEvent alarmEvent)
        {
            AlarmEvent nextAlarm= new AlarmEvent();

            SelectableDay selected = (SelectableDay)alarmEvent.SelectedDays.SelectableDayInt;

            if (selected == 0)
            {
                nextAlarm.Day = new DateTime(FromDateTime.Year, FromDateTime.Month, FromDateTime.Day, alarmEvent.TimeSet.Hours, alarmEvent.TimeSet.Minutes, alarmEvent.TimeSet.Seconds);

                if (alarmEvent.TimeSet<FromDateTime.TimeOfDay)
                {
                   nextAlarm.Day= nextAlarm.Day.AddDays(1);
                }

                _nextAlarmList.Add(nextAlarm);
            }

            else
            {
                foreach (SelectableDay selectedDay in Enum.GetValues(typeof(SelectableDay)))
                {
                    if (selected.HasFlag(selectedDay))
                    {
                        DayOfWeek dayHasFlag = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(SelectableDay), selectedDay));
                        var selectedDateTime = Convert.ToDateTime(alarmEvent.TimeSet.ToString());

                        var alarmTime = FromDateTime;
                        alarmTime = new DateTime(FromDateTime.Year, FromDateTime.Month, FromDateTime.Day, selectedDateTime.Hour, selectedDateTime.Minute, selectedDateTime.Second);

                        int dayDiff = dayHasFlag - FromDateTime.DayOfWeek;
                        if (dayDiff < 0)
                        {
                            dayDiff += 7;
                        }

                        alarmTime = alarmTime.AddDays(dayDiff);

                        if (alarmTime.DayOfWeek == FromDateTime.DayOfWeek && alarmTime < FromDateTime)
                        {
                            alarmTime = alarmTime.AddDays(7);
                        }

                        nextAlarm.Day = alarmTime;

                        _nextAlarmList.Add(nextAlarm);
                    }
                }
            }


            var test = 1;
        }


        //private void setNextAlarm()
        //{
        //    AlarmEvent alarm = NewAlarmList.OrderBy(x => x.Day).FirstOrDefault();
        //    setToast(alarm);
        //}
    }
}