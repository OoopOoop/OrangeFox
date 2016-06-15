using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.NextAlarm
{
    [Flags]
    public enum SelectableDay
    {
        Monday = 1,
        Tuesday = 1 << 1,
        Wednesday = 1 << 2,
        Thursday = 1 << 3,
        Friday = 1 << 4,
        Saturday = 1 << 5,
        Sunday = 1 << 6
    }


    public class Alarm
    {
        public string AlarmName { get; set; }
        public string SongName { get; set; }
        public TimeSpan Time { get; set; }

       // public int SelectedDayFlags { get; set;}

        public DayOfWeek DayOfWeek { get; set; }
    }


    public class AlarmManagement
    {
        public List<Alarm> alarm;

        public AlarmManagement()
        {
            //alarm = new List<Alarm> { new Alarm { AlarmName = "Alarm1", SongName = "Song1", Time = new TimeSpan(07, 00, 00), SelectedDayFlags = 96 },
            //                          new Alarm { AlarmName = "Alarm2", SongName = "Song2", Time = new TimeSpan(22, 00, 00), SelectedDayFlags = 9},
            //                          new Alarm { AlarmName = "Alarm3", SongName = "Song3", Time = new TimeSpan(08, 30, 00), SelectedDayFlags = 4 },
            //                          new Alarm { AlarmName = "Alarm4", SongName = "Song4", Time = new TimeSpan(15, 14, 00), SelectedDayFlags = 0 }};
        }



        public Alarm createAlarms(int selectedDays, TimeSpan time, )
        {
            var alarm = new Alarm();




            return alarm;
        }



    }



    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test");
            Console.ReadLine();
        }
    }
}
