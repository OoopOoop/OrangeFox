using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace POF.NextAlarm
{
    public class AlarmEvent : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));


        //  public string AlarmName { get; set;}
        //  public SoundData SelectedSound { get; set;}
        //  public DateTime TimeSet { get; set;}
        //  public bool IsAlarmOn { get; set;}
        //  public string SnoozeTime { get; set;}
        //  public DaysData SelectedDays { get; set;}


        private string alarmName;

        public string AlarmName
        {
            get { return alarmName; }
            set { alarmName = value; OnPropertyChanged(); }
        }



        //private DateTime _timeSet;
        //public DateTime TimeSet
        //{
        //    get { return _timeSet; }
        //    set { _timeSet = value;OnPropertyChanged(); }
        //}

        private TimeSpan _timeSet;
        public TimeSpan TimeSet
        {
            get { return _timeSet; }
            set { _timeSet = value; OnPropertyChanged(); }
        }



        private bool _isAlarmOn;
        public bool IsAlarmOn
        {
            get { return _isAlarmOn; }
            set { _isAlarmOn = value; OnPropertyChanged(); }
        }

        private SelectableDay _selectedDays;
        public SelectableDay SelectedDays
        {
            get { return _selectedDays; }
            set { _selectedDays = value; OnPropertyChanged(); }
        }
    }
}
