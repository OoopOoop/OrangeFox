using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
      
        public AlarmPageViewModel()
        {
           
            TimePickerTime=new TimeSpan(14,15,00);

            Sound = new SoundSelectViewModel();

            Messenger.Default.Register<SoundData>(
              this,
              sound =>
              {
                  Sound.SelectedSound = sound;
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
