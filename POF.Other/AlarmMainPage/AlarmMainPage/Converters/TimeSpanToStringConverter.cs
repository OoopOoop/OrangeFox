
using System;
using Windows.UI.Xaml.Data;

namespace AlarmMainPage.Converters
{
    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) 
        {
            string timeFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortTimePattern;

            TimeSpan timeSpan = (TimeSpan)value;
            DateTime time = DateTime.Today.Add(timeSpan);
            return timeFormat.StartsWith("h") ? time.ToString("hh:mm tt") : time.ToString("HH:mm");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
