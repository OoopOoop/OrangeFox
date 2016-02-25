﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace POF.Shared
{
    public class PopUpWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (!(value is double)) return null;

            double multiplier = 1;
            double.TryParse(parameter as string, out multiplier);
            return Window.Current.Bounds.Width * multiplier;
        }


        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
