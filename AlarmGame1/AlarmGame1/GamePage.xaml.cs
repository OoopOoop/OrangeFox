using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Notification;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//TODO: Check if vibration is working
//TODO: Make back (arrow) button function 
//TODO: Change layout

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AlarmGame1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {

        DispatcherTimer timer = new DispatcherTimer();
        bool isSame = false;
        string randomNumbers;

        public GamePage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        { 
        }

    
        private void TextBlock_Loaded(object sender, RoutedEventArgs e)
        {
            //Display random numbers and keep input textbox hidden until random numbers are visible
            //Show random numbers for 5 sec
            //RandomNumber.Text = TextBlockNumbers.getRandomNumber();


            randomNumbers = TextBlockNumbers.getRandomNumber();
            RandomNumber.Text = randomNumbers;
            TextBoxNumberInput.Visibility = Visibility.Collapsed;
            ButtonFinish.IsEnabled = false;
            ButtonOk.Visibility = Visibility.Collapsed;

            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Tick += timer_Tick;
            timer.Start();
        }


        void timer_Tick(object sender, object e)
        {
            //Hide random numbers and show textbox for input
            timer.Stop();
            RandomNumber.Text = "";
            TextBoxNumberInput.Visibility = Visibility.Visible;
            ButtonOk.Visibility = Visibility.Visible;
        }

        private void ButtonDoAgain_Click(object sender, RoutedEventArgs e)
        {
            TextBoxNumberInput.Text = "";
            this.TextBlock_Loaded(sender, e);
        }


        //Check if numbers entered are correct
        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            isSame = TextBlockNumbers.compare(randomNumbers, TextBoxNumberInput.Text);

            if (isSame == true)
            {
                ButtonFinish.IsEnabled = true;
                TextBoxNumberInput.Text = "";
                ButtonDoAgain.IsEnabled = false;
               
            }

            else
            {
                //TODO: Check if vibration is working
                //Clean text
                VibrationDevice testVibrationDevice = VibrationDevice.GetDefault();
                testVibrationDevice.Vibrate(TimeSpan.FromSeconds(1));
                testVibrationDevice.Cancel();
                TextBoxNumberInput.Text = "";
            }
        }

        private void ButtonFinish_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FinishPage));
        }
    }
}
