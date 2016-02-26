using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace POF.ViewModels
{
    public class DayModel
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

            if (selectedDaysFlags == daysWeekdays)
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
    }


    public class Day
    {
        public DayModel.SelectableDay EnumValue { get; set; }
        public bool IsSelected { get; set; }
        public string DisplayName => DayModel.SelectableDayToFullString(EnumValue);
    }



    public class AlarmRepeatSelection : ObservableCollection<Day>
    {

        public AlarmRepeatSelection(int selectableDayFlags) : this((DayModel.SelectableDay)selectableDayFlags)
        {
        }

        public AlarmRepeatSelection(DayModel.SelectableDay selectableDayFlags) : this()
        {
            this.initFromSelectableDayFlags(selectableDayFlags);
        }

        public AlarmRepeatSelection() : base()
        {
        }


        private void initFromSelectableDayFlags(DayModel.SelectableDay selectableDayFlags)
        {
            foreach (DayModel.SelectableDay day in Enum.GetValues(typeof(DayModel.SelectableDay)))
            {
                Add(new Day() { EnumValue = day, IsSelected = selectableDayFlags.HasFlag(day) });
            }
        }

        public DayModel.SelectableDay selectableDayFlags()
        {
            DayModel.SelectableDay selectableDayFlags = 0;
            foreach (var selectable in this.Where(x => x.IsSelected))
            {
                selectableDayFlags |= selectable.EnumValue;
            }

            return selectableDayFlags;
        }

    }





    public class DaySelectViewModel : ViewModelBase
    {
       
        public ICommand OpenPopUpCommand { get;}

        private string _selectedDayTitle;
        public string SelectedDayTitle
        {
            get { return _selectedDayTitle; }
            set { _selectedDayTitle = value; OnPropertyChanged(); }
        }

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


        private ObservableCollection<Day> _alarmRepeatSelection;

        public ObservableCollection<Day> AlarmRepeatSelection
        {
            get { return _alarmRepeatSelection; }
            set { _alarmRepeatSelection = value; OnPropertyChanged(); }
        }



        protected override void OnDataLoaded()
        {
           
        }


        public AlarmRepeatSelection AlarmSelection;
        private DayModel dayModel;

      
        public DaySelectViewModel()
        {
            SelectedDayTitle = "SetUpStandardDay";
            OpenPopUpCommand = new RelayCommand(showPopUp);
            dayModel = new DayModel();
            AlarmRepeatSelection = new AlarmRepeatSelection(dayModel.SelectedDaysFlags);
        }

       

        private void showPopUp()
        {
            IsPopUpOpen = true;
        }




    }
}
