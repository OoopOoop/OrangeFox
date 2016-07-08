using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace POF.Shared
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            
            try
            {
               
                var timeSpan = (TimeSpan)value;

                DateTime time = DateTime.Today.Add(timeSpan);
               

                return  time.ToString("t");
            }
            catch
            {
                return value;
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
