using System.Text;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace POF
{
    public static class ToastHelper
    {
        public static ToastNotification PopToast(string title, string content)
        {
            return PopToast(title, content, null, null);
        }

        public static ToastNotification PopToast(string title, string content, string tag, string group)
        {
            string xml = $@"<toast activationType='foreground'>
                                            <visual>
                                                <binding template='ToastGeneric'>
                                                </binding>
                                            </visual>
                                        </toast>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var binding = doc.SelectSingleNode("//binding");

            var el = doc.CreateElement("text");
            el.InnerText = title;

            binding.AppendChild(el);

            el = doc.CreateElement("text");
            el.InnerText = content;
            binding.AppendChild(el);

            return PopCustomToast(doc, tag, group);
        }


   


        public static ToastNotification PopCustomAlarmToast(string alarmName, string alarmTime, string musicPath, int snoozeTime)
        {
            string xml = $@"<toast activationType='foreground'>
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
                            <selection id='5' content = '5 minutes'/>
                            <selection id='10' content = '10 minutes'/>
                            <selection id='20' content = '20 minutes'/>
                            <selection id='30' content = '30 minutes'/>
                            <selection id='60' content = '1 hour'/>
                        </input>

                    <action activationType='system' arguments='snooze' hint-inputId='snoozeTime' content='snooze' />
                    <action activationType='system' arguments='dismiss' content='dismiss' />

                    </actions>
                <audio src='ms-winsoundevent:Notification.Looping.Call3' loop='true'/>
                                        </toast>";


            return null;
        }





        public static ToastNotification PopCustomToast(string xml)
        {
            return PopCustomToast(xml, null, null);
        }

        public static ToastNotification PopCustomToast(string xml, string tag, string group)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            return PopCustomToast(doc, tag, group);
        }

        [DefaultOverloadAttribute]
        public static ToastNotification PopCustomToast(XmlDocument doc, string tag, string group)
        {
            var toast = new ToastNotification(doc);

            if (tag != null)
                toast.Tag = tag;

            if (group != null)
                toast.Group = group;

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            return toast;
        }

        public static string ToString(ValueSet valueSet)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var pair in valueSet)
            {
                if (builder.Length != 0)
                    builder.Append('\n');

                builder.Append(pair.Key);
                builder.Append(": ");
                builder.Append(pair.Value);
            }

            return builder.ToString();
        }
    }
}