
using System;

namespace AlarmMainPage.Models
{
   public class AlarmModel
    {
        public int Id { get; set; }
        public string AlarmName { get; set; }
        public TimeSpan Time { get; set; }
        public string Occurrence { get; set; }
        public string Sound { get; set; }
        public string SnoozeMin { get; set; }
        public bool IsEnabled { get; set; }
        public string GameName { get; set; }

        //public AlarmModel(string time, string alarmName, string occurrence)
        //{
        //    Time = time;
        //    AlarmName = alarmName;
        //    Occurrence = occurrence;
        //}

        public AlarmModel()
        {
            Id = 0;
            AlarmName = string.Empty;
            Time = new TimeSpan( 7, 0, 0);
            Occurrence = string.Empty;
            Sound= string.Empty;
            SnoozeMin= string.Empty;
            IsEnabled = false;
            GameName= string.Empty;
        }
    }
}
