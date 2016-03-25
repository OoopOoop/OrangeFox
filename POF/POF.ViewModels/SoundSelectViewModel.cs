using GalaSoft.MvvmLight.Command;
using POF.Models;
using POF.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;

namespace POF.ViewModels
{
    public class SoundData
    {
        #region Properties

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private int _songID;

        public int SongID
        {
            get { return _songID; }
            set { _songID = value; }
        }

        private FileTypeEnum _fileType;

        public FileTypeEnum FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        #endregion Properties
    }

    public class AlarmStandardSoundSelection : ObservableCollection<SoundData>
    {
        public string SelectedSoundTitle { get; set; }

        public AlarmStandardSoundSelection(List<SoundFile> modelSoundCollection, string selectedSoundName) : this()
        {
            this.addSounds(modelSoundCollection);

            try
            {
                StorageFolder local = ApplicationData.Current.LocalFolder;
                if (local != null)
                {
                    //get the folder in LocalFolder
                    var dataFolder = local.GetFolderAsync("AlarmSoundFolder").AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                    //get the file
                    var file = dataFolder.TryGetItemAsync(selectedSoundName).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                    this.Add(new SoundData() { Title = file.Name, FilePath = file.Path, FileType = FileTypeEnum.Custom });
                    SelectedSoundTitle = file.Name;

                    //SelectedSoundByName(selectedSoundName);
                }
            }
            catch (Exception)
            {
                SelectedSoundTitle = "Bird Box";
            }

        }



        public AlarmStandardSoundSelection(List<SoundFile> modelSoundCollection) : base()
        {
            this.addSounds(modelSoundCollection);
        }

        public AlarmStandardSoundSelection() : base()
        {
        }

        private void addSounds(List<SoundFile> collection)
        {
            foreach (SoundFile item in collection)
            {
                this.Add(new SoundData() { FilePath = item.FilePath, Title = item.Title, FileType = item.FileType });
            }
        }

        //public string SelectedSoundByName(string name)
        //{
        //    var selectable = from i in this.Items.Cast<SoundData>()
        //                     where i.Title == name
        //                     select i;

        //    foreach (var item in selectable)
        //    {
        //        SelectedSoundTitle = item.Title;
        //    }

        //    return SelectedSoundTitle;
        //}
    }

    /// <summary>
    /// takes given sound path and checks if it was not removed from the directory
    /// </summary>
    public class AlarmCustomSoundSelection : ObservableCollection<SoundData>
    {
        public string SelectedSoundTitle { get; set; }

        public AlarmCustomSoundSelection(string fileName) : this()
        {
            try
            {
                //Task.Run(() => FindSoundFile(filePath)).Wait();
                //  StorageFile file = StorageFile.GetFileFromPathAsync(fileName).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();

                StorageFolder local = ApplicationData.Current.LocalFolder;
                if(local!=null)
                {
                    //get the folder in LocalFolder
                    var dataFolder = local.GetFolderAsync("AlarmSoundFolder").AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                    //get the file
                    var file = dataFolder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                    this.Add(new SoundData() { Title = file.Name, FilePath = file.Path, FileType = FileTypeEnum.Custom });
                    SelectedSoundTitle = file.Name;
                }

            }
            catch (Exception)
            {
                return;
            }
        }

        public AlarmCustomSoundSelection() : base()
        {
        }
    }

    /// <summary>
    /// SelectedSound will be saved and serialized, if user have not chosen sound, it will be saved with preSetSoundPath.
    /// </summary>
    public class SelectedSound
    {
        public string Name { get; set;}
      //  public string Path { get; set; }
        public FileTypeEnum FileType { get; set; }
    }

    public class SoundSelectViewModel : ViewModelBase
    {
        // private const string defaultSoundPath = "ms-appx:///Assets/Ringtones/Bird Box.wma";
        // if user have not selected sound, standard sound is "ms-winsoundevent:Notification.Looping.Alarm2", SelectedSound.Name = "ms-winsoundevent:Notification.Looping.Alarm2"?

        private string titlePlayingNow = "";

        public SelectedSound SelectedSound { get; set; }
        
        public StorageFile StoreToLocalSound { get; set;}


        private string _selectedSoundTitle;

        public string SelectedSoundTitle
        {
            get { return _selectedSoundTitle; }
            set { _selectedSoundTitle = value; OnPropertyChanged(); }
        }

        private bool isPopUpOpen;

        public bool IsPopUpOpen
        {
            get { return isPopUpOpen; }
            set { isPopUpOpen = value; OnPropertyChanged(); }
        }

        private double _popUpHeight;

        public double PopUpHeight
        {
            get { return _popUpHeight; }
            set { _popUpHeight = value; OnPropertyChanged(); }
        }

        private double _popUpWidth;

        public double PopUpWidth
        {
            get { return _popUpWidth; }
            set { _popUpWidth = value; OnPropertyChanged(); }
        }

        private MediaElement _player;

        public MediaElement Player
        {
            get { return _player; }
            set { _player = value; OnPropertyChanged(); }
        }

        private AlarmStandardSoundSelection alarmStandardSoundSelection;

        public AlarmStandardSoundSelection AlarmStandardSoundSelection
        {
            get { return alarmStandardSoundSelection; }
            set { alarmStandardSoundSelection = value; OnPropertyChanged(); }
        }

        private AlarmCustomSoundSelection alarmCustomSoundSelection;

        public AlarmCustomSoundSelection AlarmCustomSoundSelection
        {
            get { return alarmCustomSoundSelection; }
            set { alarmCustomSoundSelection = value; OnPropertyChanged(); }
        }

        public ICommand OpenPopUpCommand { get; }
        public ICommand PickCustomSoundCommand { get; }
        public ICommand SelectedSoundCommand { get; }
        public ICommand PlaySoundCommand { get; }
        public ICommand PopUpUnloadedCommand { get; }

        /// <summary>
        /// loading with fake data with path for the sound that user chose. 0916.wav from bigsoundbank.com (barking dog)
        /// </summary>
        public SoundSelectViewModel()
        {
            //  SelectedSound = new SelectedSound() { Path = "ms - appx:/Assets/Ringtones/Horizon.wma", FileType = FileTypeEnum.Uri };
            //  SelectedSound = new SelectedSound() { Path = "C:\\Data\\Users\\ Public\\ Downloads\\0897.wav", FileType = FileTypeEnum.Custom };
            //  SelectedSound.Path = Regex.Replace(SelectedSound.Name, @"\s+", "");


            //SelectedSound = new SelectedSound() { Name = "0897.wav", FileType = FileTypeEnum.Custom };
              SelectedSound = new SelectedSound() { Name = "Horizon.wma", FileType = FileTypeEnum.Uri };

            loadSounds();
            Player = new MediaElement();
            PlaySoundCommand = new RelayCommand<object>(playSound);
            OpenPopUpCommand = new RelayCommand(() => IsPopUpOpen = true);
            PickCustomSoundCommand = new RelayCommand(selectCustomSound);
            SelectedSoundCommand = new RelayCommand<object>(saveNewSelectedSound);
            PopUpUnloadedCommand = new RelayCommand(() => Player.Stop());
        }

        protected override void OnDataLoaded()
        {
        }

        private void loadSounds()
        {
            if (SelectedSound.FileType == FileTypeEnum.Custom)
            {
                AlarmCustomSoundSelection = new AlarmCustomSoundSelection(SelectedSound.Name);
                if (AlarmCustomSoundSelection.Count != 0)
                {
                    AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles);
                    SelectedSoundTitle = AlarmCustomSoundSelection.SelectedSoundTitle;
                }
                //else
                //{
                //    AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles, preSetSoundPath);
                //    SelectedSoundTitle = AlarmStandardSoundSelection.SelectedSoundTitle;
                //}
            }
            else
            {
                AlarmCustomSoundSelection = new AlarmCustomSoundSelection();
                AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles, SelectedSound.Name);
                SelectedSoundTitle = AlarmStandardSoundSelection.SelectedSoundTitle;
            }
        }

        /// <summary>
        /// if chosen sound is of type Uri(Assets), replace it's path and save in LocalFolder, if sound is suctom just contunie with it saving to LocalFolder
        /// save file to LocalFolder only when alarm entry was saved
        /// </summary>
        /// <param name="obj"></param>
        private async void saveNewSelectedSound(object obj)
        {
            var sound = obj as SoundData;
            // SelectedSound.Path = sound.FilePath;
            SelectedSound.FileType = sound.FileType;


            //TODO: Save to LocalFolder if alarm is saved
            if(sound.FileType==FileTypeEnum.Uri)
            {
                string path=sound.FilePath.Replace("ms-appx:/", "ms-appx:///");
                StoreToLocalSound = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));   
            }

            SelectedSound.Name = StoreToLocalSound.Name;

            StorageFolder local = ApplicationData.Current.LocalFolder;
            var dataFolder = await local.CreateFolderAsync("AlarmSoundFolder",
            CreationCollisionOption.OpenIfExists);
            await  StoreToLocalSound.CopyAsync(dataFolder, SelectedSound.Name, NameCollisionOption.ReplaceExisting);

            SelectedSoundTitle = sound.Title;
            IsPopUpOpen = false;
        }

        /// <summary>
        /// check if same sound is playing when clicked Play button, if so - stop player.
        /// </summary>
        /// <param name="obj"></param>
        private void playSound(object obj)
        {
            var sound = obj as SoundData;

            if (sound == null)
            {
                Player.Stop();
                return;
            }

            if (Player.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing && sound.Title == titlePlayingNow)
            {
                Player.Stop();
            }
            else if (sound.FileType == FileTypeEnum.Uri)
            {
                playStandardsound(sound.FilePath);
                titlePlayingNow = sound.Title;
            }
            else if (sound.FileType == FileTypeEnum.Custom)
            {
                playCustomSound(sound.Title);
                titlePlayingNow = sound.Title;
            }
        }

        private void playStandardsound(string path)
        {
            Player.Source = new Uri(path, UriKind.RelativeOrAbsolute);
        }

        private async void playCustomSound(string title)
        {
            foreach (AccessListEntry item in StorageApplicationPermissions.FutureAccessList.Entries)
            {
                if (item.Metadata == title)
                {
                    StorageFile actualFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(item.Token);
                    var stream = await actualFile.OpenAsync(FileAccessMode.Read);
                    Player.SetSource(stream, actualFile.ContentType);
                }
            }
        }

        /// <summary>
        /// same song can't be added twice
        /// </summary>
        /// <param name="file"></param>
        private void addCustomSong(StorageFile file)
        {
            if (AlarmCustomSoundSelection.Any(x => x.FilePath == file.Path))
            {
                return;
            }
            else
            {
                AlarmCustomSoundSelection.Add(new SoundData { Title = file.DisplayName, FilePath = file.Path, FileType = FileTypeEnum.Custom });
            }
        }

        /// <summary>
        /// Pick up custom sound from HomeGroup folders, add to popUp collection and futureAccess list
        /// </summary>
        async private void selectCustomSound()
        {
            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.HomeGroup;
            filePicker.ViewMode = PickerViewMode.List;

            filePicker.FileTypeFilter.Clear();

            filePicker.FileTypeFilter.Add(".mp3");
            filePicker.FileTypeFilter.Add(".wav");
            filePicker.FileTypeFilter.Add(".wma");

            StorageFile storageFile = await filePicker.PickSingleFileAsync();

            if (storageFile != null)
            {
                //var stream = await storageFile.OpenAsync(FileAccessMode.Read);
             
                StoreToLocalSound = storageFile;

                addCustomSong(storageFile);

                StorageApplicationPermissions.FutureAccessList.Add(storageFile, storageFile.DisplayName);
            }
        }
    }
}