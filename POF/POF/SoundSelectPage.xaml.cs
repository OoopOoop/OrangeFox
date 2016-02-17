using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace POF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SoundSelectPage : Page
    {
        public SoundSelectPage()
        {
            this.InitializeComponent();
        }

        //private void SoundSelectionPopUp_LayoutUpdated(object sender, object e)
        //{
        //    if (PopUpBorder.ActualWidth == 0 && PopUpBorder.ActualHeight == 0)
        //    {
        //        return;
        //    }

        //    double ActualHorizontalOffset = this.SoundSelectionPopUp.HorizontalOffset;
        //    double ActualVerticalOffset = this.SoundSelectionPopUp.VerticalOffset;

        //    double NewHorizontalOffset = (Window.Current.Bounds.Width - PopUpBorder.ActualWidth) / 2;
        //    double NewVerticalOffset = (Window.Current.Bounds.Height - PopUpBorder.ActualHeight) / 2;

        //    if (ActualHorizontalOffset != NewHorizontalOffset || ActualVerticalOffset != NewVerticalOffset)
        //    {
        //        this.SoundSelectionPopUp.HorizontalOffset = NewHorizontalOffset;
        //        this.SoundSelectionPopUp.VerticalOffset = NewVerticalOffset;
        //    }
        //}
    }
}
