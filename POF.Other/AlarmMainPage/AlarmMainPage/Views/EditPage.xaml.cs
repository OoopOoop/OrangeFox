
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AlarmMainPage.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPage : Page
    {
        
        public EditPage()
        {
            this.InitializeComponent();

            // DataContext = App.ViewModel;
            DataContext = App.DataModel;
        }

        

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string message = (string)e.Parameter;

            if(message=="edit")
            {
                editTxt.Text = message;

                //Adding "delete" AppBarbutton
                AppBarButton buttonToAdd = new AppBarButton { Label = "delete", Icon = new SymbolIcon(Symbol.Delete) };
                (BottomAppBar as CommandBar).PrimaryCommands.Add(buttonToAdd);


                //TODO: create event on click or bind it to a command

                //buttonToAdd.Click += async (sender, e) => await new MessageDialog("Button clicked").ShowAsync();

                //Binding myBind = new Binding();
                //myBind.Path = new PropertyPath("GoToAddSampleItemssPageCommand");
                //myBind.Source = DataContext;
                //buttonToAdd.SetBinding(AppBarButton.CommandProperty, myBind);
            }

            else
            {
                editTxt.Text = "new";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
