using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace SoundChooserToUniversal.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)=> (bool)value==false ? Visibility.Collapsed : Visibility.Visible;
      
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
           
            var visiblity = (Visibility)value;
            return visiblity == Visibility.Visible;
        }
    }
}
