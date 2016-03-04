
using AlarmMainPage.Commands;
using AlarmMainPage.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using AlarmMainPage.Views;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Windows.Storage;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace AlarmMainPage.ViewModels
{
   public class MainViewModel:ViewModelBase 
    {
       
        private AlarmModel alarm;

        private DelegateCommand<string> addAlarmCommand;
        private DelegateCommand<string> deleteAlarmCommand;
        private DelegateCommand<string> saveAlarmCommand;
        private DelegateCommand<string> showGameDemoCommand;


        private ObservableCollection<AlarmModel> alarms;
        public ObservableCollection<AlarmModel> Alarms
        {
            get { return alarms; }
            set
            {
                alarms = value;
                RaisePropertyChanged(nameof(Alarms));
            }
        }

        //public bool isDataLoaded { get; private set; }

        int AlarmCount = 0;

        #region Public properties

        public int Id
        {
            get { return alarm.Id; }
            set { alarm.Id = value;}

        }


        public string AlarmName
        {
            get { return alarm.AlarmName; }
            set
            {
                alarm.AlarmName = value;
                RaisePropertyChanged(nameof(AlarmName));
            }
        }

        /// <summary>
        /// TODO: change to ObservableCollection<TimeSpan>?
        /// </summary>
        public TimeSpan Time
        {
            get { return alarm.Time; }
            set
            {
                alarm.Time = value;
                RaisePropertyChanged(nameof(Time));
            }
        }

        /// <summary>
        /// TODO: Change to enum?
        /// </summary>
        public string Occurrence
        {
            get { return alarm.Occurrence; }
            set
            {
                alarm.Occurrence = value;
                RaisePropertyChanged(nameof(Occurrence));
            }
        }


        public string Sound
        {
            get { return alarm.Sound; }
            set
            {
                alarm.Sound = value;
                RaisePropertyChanged(nameof(Sound));
            }
        }


        public string SnoozeMin
        {
            get { return alarm.SnoozeMin; }
            set
            {
                alarm.SnoozeMin = value;
                RaisePropertyChanged(nameof(SnoozeMin));
            }
        }


        public bool IsEnable
        {
            get { return alarm.IsEnabled; }
            set
            {
                alarm.IsEnabled = value;
                RaisePropertyChanged(nameof(IsEnable));
            }
        }


        public string GameName
        {
            get { return alarm.GameName; }
            set
            {
                alarm.GameName = value;
                RaisePropertyChanged(nameof(GameName));
            }
        }

        #endregion

        [IgnoreDataMember]
        public ICommand AddAlarmCommand
        {
            get
            {
                if(addAlarmCommand==null)
                {
                    addAlarmCommand = new DelegateCommand<string>(AddAlarm, CanAddAlarm);
                }
                return addAlarmCommand;
            }
        }

        private bool CanAddAlarm(string arg) => true;

        [IgnoreDataMember]
        public ICommand DeleteAlarmCommand
        {
            get
            {
                if (deleteAlarmCommand == null)
                {
                    deleteAlarmCommand = new DelegateCommand<string>(DeleteAlarm, CanDeleteAddAlarm);
                }
                return deleteAlarmCommand;
            }
        }

        private bool CanDeleteAddAlarm(string arg) => true;


        [IgnoreDataMember]
        public ICommand SaveAlarmCommand
        {
            get
            {
                if (saveAlarmCommand == null)
                {
                    saveAlarmCommand = new DelegateCommand<string>(SaveAlarm, CanSaveAddAlarm);
                }
                return saveAlarmCommand;
            }
        }

        private bool CanSaveAddAlarm(string arg) => true;


        [IgnoreDataMember]
        public ICommand ShowGameDemoCommand
        {
            get
            {
                if (showGameDemoCommand  == null)
                {
                    showGameDemoCommand = new DelegateCommand<string>(ShowGame, CanShowGame);
                }
                return showGameDemoCommand;
            }
        }

        private bool CanShowGame(string arg) => true;
       


        public MainViewModel()
        {
            alarm = new AlarmModel();
            Alarms = new ObservableCollection<AlarmModel>();

        }



       /// <summary>
       /// Saving a new instance of alarm, or updating excisting. So check if alarm is new or already is in collection
       /// </summary>
       /// <param name="obj"></param>
        private void SaveAlarm(string obj)
        {
            // int alarmCount=0;
            //Alarms.Add(new AlarmModel() { Id = alarmCount++, Time = Time, Occurrence = "Mon, Tue, Wed", AlarmName = "Alarm1" });


            addAlarms();
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(MainPage));
        }





        private void DeleteAlarm(string obj)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Redirect to "new" page for adding new alarm
        /// </summary>
        /// <param name="obj"></param>
        private void AddAlarm(string obj)
        {
            var frame = (Frame)Window.Current.Content;
            frame.Navigate(typeof(EditPage),obj);
            
        }


        private void ShowGame(string obj)
        {
            throw new NotImplementedException();
        }




        public void LoadAlarms()
        {

            Alarms.GetEnumerator();   
            //TODO: load list of itemviewmodel
            //Items.Add(new AlarmViewModel() { Id = "0", Time = "08:00", Occurrence = "Mon, Tue", AlarmName = "Alarm1" });
            //Items.Add(new AlarmViewModel() { Id = "1", Time = "09:00", Occurrence = "Mon, Wed, Thur, Fri, Sat, Sun", AlarmName = "Alarm2" });
            //Items.Add(new AlarmViewModel() { Id = "1", Time = "10:00", Occurrence = "Mon, Tue", AlarmName = "Alarm3" });
            //Items.Add(new AlarmViewModel() { Id = "1", Time = "11:00", Occurrence = "Mon, Thur", AlarmName = "Alarm4" });
            //Items.Add(new AlarmViewModel() { Id = "1", Time = "12:00", Occurrence = "Weekends", AlarmName = "Alarm5" });
            //Items.Add(new AlarmViewModel() { Id = "1", Time = "13:00", Occurrence = "Mon, Sat", AlarmName = "Alarm6" });
            //Items.Add(new AlarmViewModel() { Id = "1", Time = "14:00", Occurrence = "Weekdays", AlarmName = "Alarm72" });
            //Items.Add(new AlarmViewModel() { Id = "1", Time = "15:00", Occurrence = "Mon,Tue, Wed, Thur, Fri, Sat, Sun", AlarmName = "Alarm8" });

            //isDataLoaded = true;
        }



        const string fileName = "alarms.json";


        public async void addAlarms()
        {
            var alarm = new AlarmModel();
            //alarm.Id = Id;
            //alarm.AlarmName = AlarmName;
            //alarm.Occurrence = Occurrence;
            //alarm.SnoozeMin = SnoozeMin;
            //alarm.Sound = Sound;
            //alarm.Time = Time;
            //alarm.IsEnabled = IsEnable;
            //alarm.GameName = GameName;

            //Test
            alarm.Id = AlarmCount++;
            alarm.Time = Time;
            alarm.AlarmName = "Alarm test";
            alarm.Occurrence = "Mon, Tue, Wed";

            Alarms.Add(alarm);

            await saveAlarmDataAsync();
        }



        private async Task saveAlarmDataAsync()
        {
            var jsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<AlarmModel>));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(fileName, CreationCollisionOption.ReplaceExisting))
            {
                jsonSerializer.WriteObject(stream, Alarms);
            }
        }




        public async Task<ObservableCollection<AlarmModel>> GetAlarms()
        {
            await ensureDataLoaded();
            //isDataLoaded = true;
            return Alarms;
        }



        private async Task ensureDataLoaded()
        {
            if(Alarms.Count==0)
            {
                await getAlarmsDataAsync();
            }
            return;
        }





        private async Task getAlarmsDataAsync()
        {
            if (Alarms.Count != 0)
            {
                return;
            }

            var JsonSerializer = new DataContractJsonSerializer(typeof(ObservableCollection<AlarmModel>));

            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(fileName))
                {
                    Alarms = (ObservableCollection<AlarmModel>)JsonSerializer.ReadObject(stream);
                }
            }

            catch
            {
                Alarms = new ObservableCollection<AlarmModel>();
            }

        }

    }
}
