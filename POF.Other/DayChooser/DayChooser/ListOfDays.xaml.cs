using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace DayChooser
{
    public sealed partial class ListOfDays : Page
    {
        private DayModel dayModel;
        private AlarmRepeatSelection alarmRepeatSelection;


       

        public ListOfDays()
        {
            this.InitializeComponent();
           
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
          if(e.Parameter is DayModel)
            {
                dayModel = e.Parameter as DayModel;
                alarmRepeatSelection = new AlarmRepeatSelection(dayModel.SelectedDaysFlags);
                DataContext = alarmRepeatSelection;
                this.setLlistBoxSelectionStatus();
            }

            base.OnNavigatedTo(e);
        }

        private void setLlistBoxSelectionStatus()
        {
            //Get all the items from our data binding that is already selected
            var alreadySelected = from i in daysListBox.Items.Cast<Day>()
                                  where i.IsSelected
                                  select i;

            //Update the view so it displays selected 
            foreach (Day day in alreadySelected)
            {
                daysListBox.SelectedItems.Add(day);
            }
        }

        private delegate void statusUpdate(IList<object> changedItems, bool newStatus);

        private void daysListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //We need the same code once for the collection of added items (selected)
            //Then again for the collection of removed items (de-selected)
            statusUpdate setter = delegate (IList<object> changedItems, bool newStatus)
            {
                if (changedItems?.Count > 0 && changedItems[0] != null)
                {
                    //Some reason not all items are typeof(SelectableDay) - I don't know why
                    foreach (var item in changedItems)
                    {
                        if (item is Day)
                            ((Day)item).IsSelected = newStatus;
                    }
                }
            };

            setter(e.AddedItems, true);
            setter(e.RemovedItems, false);
        }

        
    }
}
