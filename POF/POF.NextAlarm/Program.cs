﻿using System;
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
        public DayOfWeek DayOfWeek { get; set; }


       public string DayName { get; set; }
    }


    public class AlarmManagement
    {
        public List<Alarm> alarmList { get; set; }
        public SelectableDay EnumValue { get; set; }
    

        public AlarmManagement()
        {
            //alarm = new List<Alarm> { new Alarm { AlarmName = "Alarm1", SongName = "Song1", Time = new TimeSpan(07, 00, 00), SelectedDayFlags = 96 },
            //                          new Alarm { AlarmName = "Alarm2", SongName = "Song2", Time = new TimeSpan(22, 00, 00), SelectedDayFlags = 9},
            //                          new Alarm { AlarmName = "Alarm3", SongName = "Song3", Time = new TimeSpan(08, 30, 00), SelectedDayFlags = 4 },
            //                          new Alarm { AlarmName = "Alarm4", SongName = "Song4", Time = new TimeSpan(15, 14, 00), SelectedDayFlags = 0 }};


            alarmList = new List<Alarm>(); 
        }



        public Alarm createAlarms(int selectedDays, TimeSpan time, string name)
        {
            var alarm = new Alarm();
          

            SelectableDay selected = (SelectableDay)selectedDays;
          

            //TODO: create new datetime with time span and given int of selected days
            foreach (SelectableDay day in Enum.GetValues(typeof(SelectableDay)))
            {
                if(selected.HasFlag(day))
                {
                    alarmList.Add(new Alarm { AlarmName = name, Time = time, DayName = day.ToString() });
                }
               
            }


            return alarm;
        }



    }



    class Program
    {
        static void Main(string[] args)
        {
            AlarmManagement man = new AlarmManagement();
            man.createAlarms(96, new TimeSpan(07, 00, 00), "My Alarm");
        }
    }
}