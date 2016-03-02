using GalaSoft.MvvmLight.Command;
using POF.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;

namespace POF.ViewModels
{
    //public class DayModel
    //{
    //    private static DayOfWeek DayFlag2DayOfWeek(SelectableDay day) => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(SelectableDay), day));

    //    private static string SelectableDayToFormatString(SelectableDay day, string format)
    //    {
    //        var dayTime = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayFlag2DayOfWeek(day)));
    //        return dayTime.ToString(format);
    //    }

    //    // to return "Mon", "Tue"
    //    public static string SelectableDayToShortString(SelectableDay day) => SelectableDayToFormatString(day, "ddd");

       
    //    //The full string description in current culture language. so AlarmRepeat.SelectableDay.Wednesday returns "Wednesday"
    //    public static string SelectableDayToFullString(SelectableDay day) => SelectableDayToFormatString(day, "dddd");


    //    [Flags]
    //    public enum SelectableDay
    //    {
    //        Monday = 1,
    //        Tuesday = 1 << 1,
    //        Wednesday = 1 << 2,
    //        Thursday = 1 << 3,
    //        Friday = 1 << 4,
    //        Saturday = 1 << 5,
    //        Sunday = 1 << 6
    //    }

    //    private const SelectableDay daysWeekend = (SelectableDay.Saturday | SelectableDay.Sunday);
    //    private const SelectableDay daysWeekdays = (SelectableDay.Monday | SelectableDay.Tuesday | SelectableDay.Wednesday | SelectableDay.Thursday | SelectableDay.Friday);
    //    private const SelectableDay daysEveryDay = (SelectableDay.Monday | SelectableDay.Tuesday | SelectableDay.Wednesday | SelectableDay.Thursday | SelectableDay.Friday | SelectableDay.Saturday | SelectableDay.Sunday);


    //    private SelectableDay selectedDaysFlags;
    //    public SelectableDay SelectedDaysFlags
    //    {
    //        get { return selectedDaysFlags; }
    //        set
    //        {
    //            if (selectedDaysFlags < 0 || value > daysEveryDay)
    //            {
    //                throw new ArgumentOutOfRangeException();
    //            }
    //            selectedDaysFlags = value;
    //        }
    //    }


    //    private string cacheDisplayDescription;
    //    private SelectableDay cachedSelectedDays;

    //    public string DisplayDescription
    //    {
    //        get
    //        {
    //            if (cachedSelectedDays == selectedDaysFlags && !string.IsNullOrEmpty(cacheDisplayDescription))
    //                return cacheDisplayDescription;

    //            cachedSelectedDays = selectedDaysFlags;
    //            cacheDisplayDescription = ToString();

    //            return cacheDisplayDescription;
    //        }
    //    }

    //    public override string ToString()
    //    {
    //        if (selectedDaysFlags == 0)
    //            return "only once";

    //        if (selectedDaysFlags == daysWeekdays)
    //            return "weekends";

    //        if (selectedDaysFlags == daysWeekdays)
    //            return "weekdays";

    //        if (selectedDaysFlags == daysEveryDay)
    //            return "every day";

    //        var builder = new System.Text.StringBuilder();

    //        var flagResult = from day in Enum.GetValues(typeof(SelectableDay)).Cast<SelectableDay>()
    //                         where selectedDaysFlags.HasFlag(day)
    //                         select day;

    //        foreach (SelectableDay day in flagResult)
    //        {
    //            if (builder.Length > 0)
    //                builder.Append(", ");

    //            builder.Append(SelectableDayToShortString(day));
    //        }

    //        return builder.ToString();
    //    }
    //}








    public class Day:INotifyPropertyChanged
    {
        public DaySelectViewModel.SelectableDay EnumValue { get; set; }
        //public bool IsSelected { get; set;}
        public string DisplayName => DaySelectViewModel.SelectableDayToFullString(EnumValue);




        //Delete
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

    }



    public class AlarmRepeatSelection : ObservableCollection<Day>
    {

        public AlarmRepeatSelection(int selectableDayFlags) : this((DaySelectViewModel.SelectableDay)selectableDayFlags)
        {
        }

        public AlarmRepeatSelection(DaySelectViewModel.SelectableDay selectableDayFlags) : this()
        {
            this.initFromSelectableDayFlags(selectableDayFlags);
        }

        public AlarmRepeatSelection() : base()
        {
        }


        private void initFromSelectableDayFlags(DaySelectViewModel.SelectableDay selectableDayFlags)
        {
            foreach (DaySelectViewModel.SelectableDay day in Enum.GetValues(typeof(DaySelectViewModel.SelectableDay)))
            {
                Add(new Day() { EnumValue = day, IsSelected = selectableDayFlags.HasFlag(day) });
            }
        }



        public DaySelectViewModel.SelectableDay selectableDayFlags()
        {
            DaySelectViewModel.SelectableDay selectableDayFlags = 0;
            foreach (var selectable in this.Where(x => x.IsSelected))
            {
                //adding to flags, by using OR operator
                selectableDayFlags |= selectable.EnumValue;
            }

            return selectableDayFlags;
        }

    }



    public class DaySelectViewModel : ViewModelBase
    {

        private static DayOfWeek DayFlag2DayOfWeek(SelectableDay day) => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(SelectableDay), day));

        private static string SelectableDayToFormatString(SelectableDay day, string format)
        {
            var dayTime = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayFlag2DayOfWeek(day)));
            return dayTime.ToString(format);
        }

        // to return "Mon", "Tue"
        public static string SelectableDayToShortString(SelectableDay day) => SelectableDayToFormatString(day, "ddd");


        //The full string description in current culture language. so AlarmRepeat.SelectableDay.Wednesday returns "Wednesday"
        public static string SelectableDayToFullString(SelectableDay day) => SelectableDayToFormatString(day, "dddd");


        [Flags]
        public enum SelectableDay
        {
            Monday = 1,
            Tuesday = 1 << 1,
            Wednesday = 1 << 2,
            Thursday = 1 << 3,
            Friday = 1 << 4,
            Saturday = 1 << 5,
            Sunday = 1 << 6
        }

        private const SelectableDay daysWeekend = (SelectableDay.Saturday | SelectableDay.Sunday);
        private const SelectableDay daysWeekdays = (SelectableDay.Monday | SelectableDay.Tuesday | SelectableDay.Wednesday | SelectableDay.Thursday | SelectableDay.Friday);
        private const SelectableDay daysEveryDay = (SelectableDay.Monday | SelectableDay.Tuesday | SelectableDay.Wednesday | SelectableDay.Thursday | SelectableDay.Friday | SelectableDay.Saturday | SelectableDay.Sunday);


        private SelectableDay selectedDaysFlags;
        public SelectableDay SelectedDaysFlags
        {
            get { return selectedDaysFlags; }
            set
            {
                if (selectedDaysFlags < 0 || value > daysEveryDay)
                {
                    throw new ArgumentOutOfRangeException();
                }
                selectedDaysFlags = value;
            }
        }


        private string cacheDisplayDescription;
        private SelectableDay cachedSelectedDays;



        public string DisplayDescription
        {
            get
            {
                if (cachedSelectedDays == selectedDaysFlags && !string.IsNullOrEmpty(cacheDisplayDescription))
                    return cacheDisplayDescription;

                cachedSelectedDays = selectedDaysFlags;
                cacheDisplayDescription = ToString();

                return cacheDisplayDescription;
            }

        }
        

        public override string ToString()
        {
            if (selectedDaysFlags == 0)
                return "only once";

            if (selectedDaysFlags == daysWeekend)
                return "weekends";

            if (selectedDaysFlags == daysWeekdays)
                return "weekdays";

            if (selectedDaysFlags == daysEveryDay)
                return "every day";

            var builder = new System.Text.StringBuilder();

            var flagResult = from day in Enum.GetValues(typeof(SelectableDay)).Cast<SelectableDay>()
                             where selectedDaysFlags.HasFlag(day)
                             select day;

            foreach (SelectableDay day in flagResult)
            {
                if (builder.Length > 0)
                    builder.Append(", ");

                builder.Append(SelectableDayToShortString(day));
            }

            return builder.ToString();
        }


        public ICommand OpenPopUpCommand { get;}
        public ICommand SelectedDaysCommand { get; }

        private bool isPopUpOpen;

        public bool IsPopUpOpen
        {
            get { return isPopUpOpen; }
            set { isPopUpOpen = value; OnPropertyChanged(); }
        }


        private double _popUpHeight;

        public double PopUpHeight
        {
            get { return _popUpHeight; }
            set { _popUpHeight = value; OnPropertyChanged(); }
        }


        private double _popUpWidth;


        public double PopUpWidth
        {
            get { return _popUpWidth; }
            set { _popUpWidth = value; OnPropertyChanged(); }
        }



        protected override void OnDataLoaded()
        {
           
        }


        private AlarmRepeatSelection _alarmSelection;

        public AlarmRepeatSelection AlarmSelection
        {
            get { return _alarmSelection; }
            set { _alarmSelection = value; OnPropertyChanged(); }
        }
        

     
        public DaySelectViewModel()
        {
            OpenPopUpCommand = new RelayCommand(showPopUp);
            AlarmSelection = new AlarmRepeatSelection(7);
            SelectedDaysFlags = (SelectableDay) 7;
            SelectedDaysCommand = new RelayCommand<SelectionChangedEventArgs>(setSelectedDays);  
        }


        private void showPopUp()
        {
            IsPopUpOpen = true;
        }



        private delegate void _statusUpdate(IList<object> changedItems, bool newStatus);

        private void setSelectedDays(SelectionChangedEventArgs e)
        {
            _statusUpdate setter = delegate (IList<object> changedItems, bool newStatus)
            {
                if (changedItems?.Count > 0 && changedItems[0] != null)
                {
                    foreach (var item in changedItems)
                    {
                        if (item is Day)
                        {
                            ((Day)item).IsSelected = newStatus;
                         
                        }
                           
                    }
                }
            };

            setter(e.AddedItems, true);
            setter(e.RemovedItems, false);

            SelectedDaysFlags= AlarmSelection.selectableDayFlags();
            OnPropertyChanged(nameof(DisplayDescription));
           
        }
    }
}
