using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.Models
{
    public class SnoozeTime
    {
        public string SnoozeMin { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0} {1}", SnoozeMin, SnoozeMin == "1" ? "hour" : "minutes");
            return sb.ToString();
        }
    }
}
