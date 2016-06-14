using System.Text;

namespace POF.Models
{
    public  class DaysData
    {
        public int DisplayNameNum { get; set; }
        public string DisplayName { get; set; }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}", DisplayName);
            return sb.ToString();
        }
    }
}
