using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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

        public ICommand TestCommand { get; }

        public SoundSelectViewModel Sound { get; set; }

        public SoundData SelectedSound { get; set; }

        public AlarmPageViewModel()
        {
           
            TimePickerTime=new TimeSpan(14,15,00);



            SelectedSound = new SoundData()
            {
                Title = "Archipelago",
                FileType = FileTypeEnum.Uri,
                ToastFilePath = "ms-appx:///Assets/Ringtones/Archipelago.wma",
                FilePath = "C:\\Data\\Users\\DefApps\\AppData\\Local\\Packages\\66157101-a353-4f28-b29a-ddc6fe58dccc_rvrkc1hdkd6c0\\LocalState\\AlarmSoundFolder\\Archipelago.wma"
            };

          
            //when srializing
            getSound();
        }


       
        private void getSound()
        {
            Messenger.Default.Register<SoundData>(
           this,
             sound =>
             {
                  SelectedSound = sound;
             });
        }

      
    


        protected override void OnDataLoaded()
        {

        }



        private TimeSpan timePickerTime;
        public TimeSpan TimePickerTime
        {
            get { return timePickerTime; }
            set
            {
                timePickerTime = value;
                OnPropertyChanged();
                SaveNewTime();
            }
        }

        private void SaveNewTime()
        {
            var newtime = TimePickerTime;
        }

      
    }
}
