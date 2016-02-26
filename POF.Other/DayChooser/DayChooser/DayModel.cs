using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DayChooser
{
    public class Day
    {
        public DayModel.SelectableDay EnumValue { get; set; }
        public bool IsSelected { get; set; }
        public string DisplayName => DayModel.SelectableDayToFullString(EnumValue);
    }


    /// <summary>
    /// to show data in design time
    /// </summary>
    internal class DayModelSample : DayModel
    {
        public DayModelSample()
        {
            SelectedDaysFlags = (SelectableDay)4;
        }
    }


    internal class AlarmRepeatSelectionSample : AlarmRepeatSelection
    {
        public AlarmRepeatSelectionSample():base((DayModel.SelectableDay)3)
        {

        }
    }



    internal class AlarmRepeatSelection:ObservableCollection<Day>
    {
       
        public AlarmRepeatSelection(int selectableDayFlags):this((DayModel.SelectableDay)selectableDayFlags)
        {
        }

        public AlarmRepeatSelection (DayModel.SelectableDay selectableDayFlags):this()
        {
            this.initFromSelectableDayFlags(selectableDayFlags);
        }
       
        public AlarmRepeatSelection():base()
        {
        }

        private void initFromSelectableDayFlags(DayModel.SelectableDay selectableDayFlags)
        {
            foreach (DayModel.SelectableDay  day in Enum.GetValues(typeof(DayModel.SelectableDay)))
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




    public class DayModel : INotifyPropertyChanged
    {

        #region PropertyChangeHandler
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName]string propertyName="")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #region Commands

        private DelegateCommand<string> buttonPressedCommand;
        private DelegateCommand<int> dayAcceptCommand;
        private DelegateCommand<string> dayClearCommand;



        public ICommand ButtonPressedCommand
        {
            get
            {
                if (buttonPressedCommand == null)
                {
                    buttonPressedCommand = new DelegateCommand<string>(PressButton, CanPressButton);
                }
                return buttonPressedCommand;
            }
        }

        public ICommand DayAcceptCommand
        {
            get
            {
                if (dayAcceptCommand == null)
                {
                    dayAcceptCommand = new DelegateCommand<int>(AcceptDay, CanAcceptDay);
                }
                return dayAcceptCommand;
            }
        }

        public ICommand DayClearCommand
        {
            get
            {
                if (dayClearCommand == null)
                {
                    dayClearCommand = new DelegateCommand<string>(ClearDay, CanClearDay);
                }
                return dayClearCommand;
            }
        }


        private bool CanPressButton(string arg) => true;
        private bool CanAcceptDay(int arg) => true;
        private bool CanClearDay(string arg) => true;

        #endregion


        private Day day;
      
        public bool IsSelected { get { return day.IsSelected; } set { day.IsSelected = value; OnPropertyChanged(); } }
     

        /// <summary>
        /// Convert SelectableDay Enum to System.DayOfWeek
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        private static DayOfWeek DayFlag2DayOfWeek(SelectableDay day) => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Enum.GetName(typeof(SelectableDay), day));


        /// <summary>
        /// Convert DayOfWeek to a DateTime for that day of week and getdefault culture text value for day
        /// </summary>
        /// <param name="day"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        private static string SelectableDayToFormatString(SelectableDay day, string format)
        {
            var dayTime = DateTime.Today.AddDays(-(DateTime.Today.DayOfWeek - DayFlag2DayOfWeek(day)));
            return dayTime.ToString(format);
        }

        /// <summary>
        ///Converts enum SelectableDay to short string description
        /// </summary>
        /// <param name="day"></param>
        /// <returns>DayModel.SelectableDay.Wednesday returns "Wed"</returns>
        public static string SelectableDayToShortString(SelectableDay day) => SelectableDayToFormatString(day, "ddd");

        /// <summary>
        /// Converts DayModel.SelectableDay Enum to full string description
        /// </summary>
        /// <param name="day"></param>
        /// <returns>The full string description in current culture language. so AlarmRepeat.SelectableDay.Wednesday returns "Wednesday"</returns>
        public static string SelectableDayToFullString(SelectableDay day) => SelectableDayToFormatString(day, "dddd");


        [Flags]
        public enum SelectableDay
        {
            Monday=1,
            Tuesday=1<<1,
            Wednesday=1<<2,
            Thursday=1<<3,
            Friday=1<<4,
            Saturday=1<<5,
            Sunday=1<<6
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
                if(selectedDaysFlags<0||value>daysEveryDay)
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

            if (selectedDaysFlags == daysWeekdays )
                return "weekends";

            if (selectedDaysFlags == daysWeekdays )
                return "weekdays";

            if (selectedDaysFlags == daysEveryDay )
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



      
        private void PressButton(string str)
        { 
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(ListOfDays),this);
        }


        private void AcceptDay(int newSelectedFlags)
        {
            this.SelectedDaysFlags = (SelectableDay) newSelectedFlags;
            var frame = (Frame)Window.Current.Content;
            frame.GoBack();
        }


        private void ClearDay(string str)
        {
            var frame = (Frame)Window.Current.Content;
            frame.GoBack();
        }
    }
}
