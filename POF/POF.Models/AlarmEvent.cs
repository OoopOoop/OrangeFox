﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.Models
{
  public class AlarmEvent
    {
        public string AlarmName { get; set;}
        public Uri SoundPath { get; set; }
        public DateTime TimeSet { get; set;}
    }
}