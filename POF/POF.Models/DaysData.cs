using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.Models
{
   public  class DaysData
    {
        public int DaysInt { get; set; }
        public string DaysString { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}", DaysString);
            return sb.ToString();
        }
    }
}
