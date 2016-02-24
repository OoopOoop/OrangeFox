using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace DayChooser
{
    public sealed partial class MainPage : Page
    {
        private DayModel dayModel;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            if(e.Parameter is DayModel)
            {
                dayModel = e.Parameter as DayModel;
                DataContext = dayModel;
            }

            base.OnNavigatedTo(e);
        }
    }
}
