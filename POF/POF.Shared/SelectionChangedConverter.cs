using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace POF.Shared
{
    public class SelectionChangedConverter : IValueConverter
    {
        public object SelectedSound { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var listView = parameter as ListView;

            //if (listView.Name == "DaysOfWeekList")
            //{
            //    return listView.SelectedItems;
            //}
            //else
            //{
                SelectedSound = listView.SelectedItem ?? SelectedSound;

                listView.SelectedItem = null;

                return SelectedSound;
            //}
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}