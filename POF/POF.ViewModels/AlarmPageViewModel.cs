using GalaSoft.MvvmLight.Command;
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

      

 
        public AlarmPageViewModel()
        {
            TestCommand = new RelayCommand<object>(testMethod);

            TimePickerTime=new TimeSpan(14,15,00);
        }

        private void testMethod(object obj)
        {
           
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
