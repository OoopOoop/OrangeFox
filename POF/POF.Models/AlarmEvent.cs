using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.Models
{
  public class AlarmEvent
    {
        public string AlarmName { get; set;}

        //replace with sound name ("sound.wav") that stored in local folder?
        // public Uri SoundPath { get; set; }

        public SoundData SelectedSound { get; set;}

        public DateTime TimeSet { get; set;}
        public bool IsOn { get; set;}
        public string SnoozeTime { get; set;}
        public SelectedDaysData SelectedDays { get; set;}
    }
}
