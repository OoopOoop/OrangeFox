using POF.Shared;
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
    public sealed partial class SoundsPopUpControl : UserControl
    {
        public SoundsPopUpControl()
        {
            this.InitializeComponent();
            var dataContext = new SoundSelectViewModel();
            this.DataContext = dataContext;
        }


       
        // put in center
        private void SoundSelectionPopUp_LayoutUpdated(object sender, object e)
        {
            if (PopUpBorder.ActualWidth == 0 && PopUpBorder.ActualHeight == 0)
            {
                return;
            }


            double ActualHorizontalOffset = this.SoundSelectionPopUp.HorizontalOffset;
            double ActualVerticalOffset = this.SoundSelectionPopUp.VerticalOffset;


            double NewHorizontalOffset = (Window.Current.Bounds.Width - PopUpBorder.ActualWidth) / 2-10;
            double NewVerticalOffset = (Window.Current.Bounds.Height - PopUpBorder.ActualHeight) / 2-10;

          
            if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
            {
                this.SoundSelectionPopUp.HorizontalOffset = NewHorizontalOffset;
                this.SoundSelectionPopUp.VerticalOffset = NewVerticalOffset;
            }
        }



        //TODO: bind to IsOpen. Change page DataContext to Local, bind IsPopUpOpen to local property on AlarmPage, so can try open poup with buttons click

        public bool IsPopUpOpen
        {
            get { return (bool)GetValue(IsPopUpOpenProperty); }
            set { SetValue(IsPopUpOpenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsPopUpOpen.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsPopUpOpenProperty =
            DependencyProperty.Register("IsPopUpOpen", typeof(bool), typeof(SoundsPopUpControl), new PropertyMetadata(0));


    }
}
