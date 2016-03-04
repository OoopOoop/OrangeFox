using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace AlarmMainPage.Converters
{
    public class ContentToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (string)parameter == "On" ? $"{"On:"} {value}" : $"{"Off:"} {value}";

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
