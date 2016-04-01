using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace POF.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public string AudioName { get; set;}

        private bool alarmIsOn;
        public bool AlarmIsOn
        {
            get
            {
                return alarmIsOn;
            }
            set
            {
                if(alarmIsOn!=value)
                {
                    alarmIsOn = value;
                    if(alarmIsOn)
                    {
                        InvokeToast();
                    }
                }
                alarmIsOn = value; OnPropertyChanged();
            }
        }
      
        private void InvokeToast()
        {
            ToastNotificationManager.History.Clear();

            setToast("MorningAlarm", "10:38 PM", Path, "30");
        }


        private void setToast(string alarmName, string alarmTime, string soundPath, string snoozeTime)
        {
         

            string xml = $@"<toast activationType='foreground' scenario='reminder' launch='args'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                <image placement='AppLogoOverride' src='Assets/Alarm_Icon.png'/> 
                                                <text>Alarm</text>
                                                <text>{alarmName}</text> 
                                                <text>{alarmTime}</text>
                                                </binding>
                                            </visual>
                    <actions>
                       <input id='snoozeTime' type='selection' defaultInput='{snoozeTime}'>
                            <selection id='5' content  = '5 minutes'/>
                            <selection id='10' content = '10 minutes'/>
                            <selection id='20' content = '20 minutes'/>
                            <selection id='30' content = '30 minutes'/>
                            <selection id='60' content = '1 hour'/>
                        </input>
                    <action activationType='system' arguments='snooze' hint-inputId='snoozeTime' content='' />
                    <action activationType='system' arguments='dismiss' content='' />
                    </actions>
                <audio src='{soundPath}' loop='true'/>
                                        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            var toast = new ToastNotification(doc);
            ToastNotificationManager.CreateToastNotifier().Show(toast);

            AlarmIsOn = false;
        }

    

        //TODO: add checking for blank spaces
        public string Path { get; set; }

        public MainPageViewModel()
        {
            Messenger.Default.Register<SoundData>(
                this,
                sound =>
            {
                Path = sound.ToastFilePath;
            });
        }


        protected override void OnDataLoaded()
        {
           
        }
    }
}
