using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace POF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AlarmPage));
        }


        private void Initialize()
        {
            // Clear all existing notifications
            ToastNotificationManager.History.Clear();

            string time = "60";
            string alarmName = "Good Morning";
            string path = "ms-appx:///Assets/Ringtones/Good Times.wma";

          
            popToastControl.Payload =
            $@"
                <toast activationType='foreground' launch='args' scenario='reminder'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <image placement='AppLogoOverride' src='Assets/Alarm_Icon.png'/> 
                            <text>Alarm</text>
                            <text>{alarmName}</text> 
                            <text>9:28 PM</text>
                        </binding>
                    </visual>
                    <audio src='{path}'/>
                    <actions>
                       <input id='snoozeTime' type='selection' defaultInput='{time}'>
                            <selection id='5' content = '5 minutes'/>
                            <selection id='10' content = '10 minutes'/>
                            <selection id='20' content = '20 minutes'/>
                            <selection id='30' content = '30 minutes'/>
                            <selection id='60' content = '1 hour'/>
                        </input>
                    <action activationType='system' arguments='snooze' hint-inputId='snoozeTime' content='' />
                    <action activationType='system' arguments='dismiss' content='dismiss' />
                    </actions>
                       </toast>";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
        }
    }
}
