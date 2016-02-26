using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.Devices.Notification;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace AlarmGame2Rectangles
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        string targetName;

        List<string> randomRectanles = new List<string>();

        RandomRect random = new RandomRect();

        Color blue = Color.FromArgb(255, 18, 18, 232);
        Color red = Color.FromArgb(255, 232, 18, 18);


        int recTapped = 0;
        int blueTapped = 0;

        //Bind it to game roperties where you can set difuculty and buzzing options
        int allowedMistakes = 0;
        bool buzzing;


        List<Rectangle> rectangles = new List<Rectangle>();


        public GamePage()
        {
            this.InitializeComponent();

            rectangles.Add(rec1);
            rectangles.Add(rec2);
            rectangles.Add(rec3);
            rectangles.Add(rec4);
            rectangles.Add(rec5);
            rectangles.Add(rec6);
            rectangles.Add(rec7);
            rectangles.Add(rec8);
            rectangles.Add(rec9);



            //flipBegin();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void Button_Tapped(object sender, TappedRoutedEventArgs e)
        {
            flipBegin();
        }



        private void rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            recTapped++;

            targetName = (sender as Rectangle).Name;

            string targetStoryboard = targetName + "Tap";


            //Check if blue rec was tappped
            if (targetName == randomRectanles[0] || targetName == randomRectanles[1] || targetName == randomRectanles[2])
            {
                blueTapped++;
            }


            Storyboard tapped = this.FindName(targetStoryboard) as Storyboard;
            tapped.Begin();


            //Unsubscribing a single rec from an event that allows it to be flipped from touch
            Rectangle rec = sender as Rectangle;
            rec.Tapped -= rectangle_Tapped;




            if (recTapped - blueTapped > allowedMistakes)
            {
                if (buzzing)
                {
                    VibrationDevice testVibrationDevice = VibrationDevice.GetDefault();
                    testVibrationDevice.Vibrate(TimeSpan.FromSeconds(1));
                    testVibrationDevice.Cancel();
                }


                errorTextBlock.Text = "FAIL";
                Storyboard fadeOutMessage = this.FindName("fadeOutMessage") as Storyboard;
                fadeOutMessage.Begin();


                flipBegin();
            }


            else if (blueTapped == 3)
            {
                errorTextBlock.Text = "SUCCESS";
                Storyboard fadeOutMessage = this.FindName("fadeOutMessage") as Storyboard;
                fadeOutMessage.Begin();

                StoryboardFade.Begin();
            }

        }



        private void flipBegin()
        {
            //Changing color of rectangles back to red
            changeColor(red);

            randomRectanles.Clear();
            recTapped = 0;
            blueTapped = 0;

            //Unsubscribing from an event that allows rectangle to be flipped from touch
            foreach (Rectangle item in rectangles)
            {
                item.Tapped -= rectangle_Tapped;
            }


            //Choosing 3 random rectangles
            random.chooseRect();

            //Adding 3 random rectangles to List
            randomRectanles.Add(random.RecBlueOne);
            randomRectanles.Add(random.RecBlueTwo);
            randomRectanles.Add(random.RecBlueThree);

            //Changing their color to blue
            changeColor(blue);


            rec1Flip.Begin();
            rec2Flip.Begin();
            rec3Flip.Begin();
            rec4Flip.Begin();
            rec5Flip.Begin();
            rec6Flip.Begin();
            rec7Flip.Begin();
            rec8Flip.Begin();
            rec9Flip.Begin();


            //Subscribing to an event that allows rectangle to be flipped from touch
            foreach (Rectangle item in rectangles)
            {
                item.Tapped += rectangle_Tapped;

            }
        }


        //Changing properties of choosen storyboard in Tap and Flip
        private void changeColor(Color color)
        {
            foreach (string item in randomRectanles)
            {

                string name1 = item + "Flip";
                string name2 = item + "Tap";
                Storyboard storyboardFlip = this.FindName(name1) as Storyboard;
                Storyboard storyboardTap = this.FindName(name2) as Storyboard;
                ChangeRecProperty.changeColor(storyboardFlip, color);
                ChangeRecProperty.changeColor(storyboardTap, color);

            }
        }

        private void buttonFinish_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FinishPage));
        }
    }
}
