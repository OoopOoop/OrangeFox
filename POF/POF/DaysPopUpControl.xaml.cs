using POF.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace POF
{
    public sealed partial class DaysPopUpControl : UserControl
    {
        public DaysPopUpControl()
        {
            this.InitializeComponent();

        }


        //TODO: same repeating event for 2 popups -> make 1
        private void SelectionPopUp_LayoutUpdated(object sender, object e)
        {
            if (PopUpBorder.ActualWidth == 0 && PopUpBorder.ActualHeight == 0)
            {
                return;
            }

            double ActualHorizontalOffset = this.SelectionPopUp.HorizontalOffset;
            double ActualVerticalOffset = this.SelectionPopUp.VerticalOffset;


            double NewHorizontalOffset = (Window.Current.Bounds.Width - PopUpBorder.ActualWidth) / 2 - 10;
            double NewVerticalOffset = (Window.Current.Bounds.Height - PopUpBorder.ActualHeight) / 2 - 300;


            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.SelectionPopUp.HorizontalOffset = NewHorizontalOffset;
                this.SelectionPopUp.VerticalOffset = NewVerticalOffset;
            }

        }

        private void DaysOfWeekList_Loaded(object sender, RoutedEventArgs e)
        {
            var alreadySelected = from i in DaysOfWeekList.Items.Cast<Day>()
                                  where i.IsSelected
                                  select i;

            //Update the view so it displays selected 
           
            foreach (Day day in alreadySelected)
            {
                DaysOfWeekList.SelectedItems.Add(day);
            }
        }
    }
}
