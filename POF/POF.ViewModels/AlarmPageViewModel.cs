using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using POF.Models;
using System;
using System.Collections.ObjectModel;

namespace POF.ViewModels
{

    public class SnoozeTimeCollection:ObservableCollection<SnoozeTime>
    {
       public SnoozeTimeCollection()
        {
            this.Add(new SnoozeTime { SnoozeMin = "5"});
            this.Add(new SnoozeTime { SnoozeMin = "10"});
            this.Add(new SnoozeTime { SnoozeMin = "20"});
            this.Add(new SnoozeTime { SnoozeMin = "30"});
            this.Add(new SnoozeTime { SnoozeMin = "1"});
        }
    }
       

    public class AlarmPageViewModel : ViewModelBase
    {
        private const string newAlarmHeader = "NEW ALARM";
        private const string editAlarmHeader = "EDIT ALARM";

        private SnoozeTime _selectedSnoozeTime;
        public SnoozeTime SelectedSnoozeTime
        {
            get { return _selectedSnoozeTime; }
            set { _selectedSnoozeTime = value; OnPropertyChanged(); }
        }


        private SnoozeTimeCollection _snoozeTimeCollection;
        public SnoozeTimeCollection SnoozeTimeCollection
        {
            get { return _snoozeTimeCollection; }
            set { _snoozeTimeCollection = value; OnPropertyChanged(); }
        }

        

        private TimeSpan timePickerTime;
        public TimeSpan TimePickerTime
        {
            get { return timePickerTime; }
            set
            {
                timePickerTime = value;
                OnPropertyChanged();
                calculateRemainingtime();
            }
        }


        private SoundData _selectedSound { get; set; }
        private DaysData _selectedDays { get; set;}

        
        private string _alarmName;
        public string AlarmName
        {
            get { return _alarmName; }
            set { _alarmName = value; OnPropertyChanged(); }
        }



        private string _timeRemainTxt;
        public string TimeRemainTxt
        {
            get { return _timeRemainTxt; }
            set { _timeRemainTxt = value;OnPropertyChanged(); }
        }



        private string _pageHeaderTxt;
        public string PageHeaderTxt
        {
            get { return _pageHeaderTxt; }
            set { _pageHeaderTxt = value; OnPropertyChanged(); }
        }



        public RelayCommand SaveNewAlarmCommand { get; set;}


        INavigationService _navigationService;

        public AlarmPageViewModel(INavigationService navigationService)
        {
            //takes saved alarmEvent, or opens new  one

            //SelectedSound = new SoundData()
            //{
            //    Title = "Archipelago",
            //    FileType = FileTypeEnum.Uri,
            //    ToastFilePath = "ms-appx:///Assets/Ringtones/Archipelago.wma",
            //    FilePath = "C:\\Data\\Users\\DefApps\\AppData\\Local\\Packages\\66157101-a353-4f28-b29a-ddc6fe58dccc_rvrkc1hdkd6c0\\LocalState\\AlarmSoundFolder\\Archipelago.wma"
            //};


            _navigationService = navigationService;

            SnoozeTimeCollection = new SnoozeTimeCollection();
            addNewAlarm();


            //to receive data from DaySelectViewModel and SoundSelectViewmodel
            getSelectedSound();
            getSelectedDays();


            SaveNewAlarmCommand = new RelayCommand(saveNewAlarmEvent);

        }


        private void calculateRemainingtime()
        {
            string timeLeft = "";
            var timeReminder = Convert.ToDateTime(TimePickerTime.ToString());

            if(timeReminder.Date<DateTime.Now)
            {
                timeReminder=timeReminder.AddDays(1);
            }

            TimeSpan timeSpan = timeReminder - DateTime.Now;

            if(timeSpan.Hours>=1)
            {
                timeLeft = String.Format("{0} {1} {2}, {3} {4}", "In", timeSpan.Hours, timeSpan.Hours > 1 ? "hours" : "hour", timeSpan.Minutes, "minutes");
            }
            else
            {
               // timeLeft= String.Format("{0} {1} {2}", "In",timeSpan.Minutes, "minutes");
                timeLeft = $"In {timeSpan.Minutes} minutes";
            }
            TimeRemainTxt=timeLeft;
        }

        

        private void addNewAlarm()
        {
            PageHeaderTxt = newAlarmHeader;
            AlarmName = "Alarm";
            TimePickerTime = new TimeSpan(07, 00, 00);      
            // 10 min snooze as a standard
            SelectedSnoozeTime = SnoozeTimeCollection[1];
        }


        private void editAlarm()
        {
            PageHeaderTxt = editAlarmHeader;
        }



        private void saveNewAlarmEvent()
        {
            var alarm = new AlarmEvent();
            alarm.AlarmName = AlarmName;
            alarm.IsAlarmOn = true;
            alarm.SelectedSound = _selectedSound;
            alarm.TimeSet = Convert.ToDateTime(TimePickerTime.ToString());
           
            alarm.SelectedDays = this._selectedDays;

            //alarm.SnoozeTime = SelectedSnoozeTime.SnoozeMin;

            alarm.SnoozeTime = SelectedSnoozeTime;


            Messenger.Default.Send(alarm);
            _navigationService.NavigateTo("MainPage");
        }



        private void getSelectedSound()
        {
            Messenger.Default.Register<SoundData>(
           this,
             sound =>
             {
                  _selectedSound = sound;
             });
        }


        private void getSelectedDays()
        {
            Messenger.Default.Register<DaysData>(
           this,
             days =>
             {
                 _selectedDays = days;
             });
        }




        //protected override void OnDataLoaded()
        //{

        //}

        
    }
}
