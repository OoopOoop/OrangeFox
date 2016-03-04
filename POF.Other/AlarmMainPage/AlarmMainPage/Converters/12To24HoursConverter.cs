using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AlarmMainPage.Converters
{
    public class _12To24HoursConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string timeFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;
            return timeFormat.StartsWith("h") ? "12HourClock" : "24HourClock";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
