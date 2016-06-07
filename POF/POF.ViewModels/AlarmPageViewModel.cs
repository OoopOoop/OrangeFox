using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using POF.Models;
using POF.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace POF.ViewModels
{
    public class AlarmPageViewModel : ViewModelBase
    {

        //public SoundSelectViewModel SoundToSelect { get; set; }
        //public DaySelectViewModel DaysToSelect { get; set;}


        private TimeSpan timePickerTime;
        public TimeSpan TimePickerTime
        {
            get { return timePickerTime; }
            set
            {
                timePickerTime = value;
                OnPropertyChanged();
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





        public RelayCommand SaveNewAlarmCommand { get; set;}




        public AlarmPageViewModel()
        {
            //takes saved alarmEvent, or opens new  one


            // TimePickerTime=new TimeSpan(14,15,00);



            //SelectedSound = new SoundData()
            //{
            //    Title = "Archipelago",
            //    FileType = FileTypeEnum.Uri,
            //    ToastFilePath = "ms-appx:///Assets/Ringtones/Archipelago.wma",
            //    FilePath = "C:\\Data\\Users\\DefApps\\AppData\\Local\\Packages\\66157101-a353-4f28-b29a-ddc6fe58dccc_rvrkc1hdkd6c0\\LocalState\\AlarmSoundFolder\\Archipelago.wma"
            //};

            AlarmName = "Good Morning";


            //to receive data from days and sound viewmodels
            getSelectedSound();
            getSelectedDays();


            
            SaveNewAlarmCommand = new RelayCommand(saveNewAlarmEvent);

        }


        private void saveNewAlarmEvent()
        {
            var alarm = new AlarmEvent();
            alarm.AlarmName = AlarmName;
            alarm.IsOn = true;
            alarm.SelectedSound = _selectedSound;
            alarm.TimeSet = Convert.ToDateTime(TimePickerTime.ToString());
            alarm.SelectedDays = this._selectedDays;


            var testtoremove = new object();
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




        protected override void OnDataLoaded()
        {

        }

        
    }
}
