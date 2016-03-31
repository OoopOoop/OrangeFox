using GalaSoft.MvvmLight.Command;
using POF.Models;
using POF.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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

        //does not work
        public string FileName => FilePath?.Substring(FilePath.LastIndexOf(@"\"));

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        //private int _songID;

        //public int SongID
        //{
        //    get { return _songID; }
        //    set { _songID = value; }
        //}

        private FileTypeEnum _fileType;

        public FileTypeEnum FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }

        private bool _isInLocal;

        public bool IsInLocal
        {
            get { return _isInLocal; }
            set { _isInLocal = value; }
        }

        private string _toastFilePath;

        public string ToastFilePath
        {
            get { return _toastFilePath; }
            set { _toastFilePath = value; }
        }



        #endregion Properties
    }

    public class AlarmStandardSoundSelection : ObservableCollection<SoundData>
    {
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
    }

    public class AlarmCustomSoundSelection : ObservableCollection<SoundData>
    {
        public AlarmCustomSoundSelection()
        {
        }
    }

    public class SoundSelectViewModel : ViewModelBase
    {
        private string titlePlayingNow = "";

        public SoundData SelectedSound { get; set; }

        public StorageFile CustomStorageFile { get; set; }

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

        public SoundSelectViewModel()
        {
            //TODO: save standard sound to local folder if user skip step of selecting sound

            //Examples
            SelectedSound = new SoundData()
            {
                Title = "0897",
                FileType = FileTypeEnum.Custom,
                ToastFilePath = "ms-appdata:///local/AlarmSoundFolder/0897.wav",
                FilePath="C:\\Data\\Users\\DefApps\\AppData\\Local\\Packages\\66157101-a353-4f28-b29a-ddc6fe58dccc_rvrkc1hdkd6c0\\LocalState\\AlarmSoundFolder\\0897.wav"
            };

            //SelectedSound = new SoundData()
            //{
            //    Title = "Horizon.wma",
            //    FileType = FileTypeEnum.Custom,
            //    FilePath = "C:\\Data\\Users\\DefApps\\AppData\\Local\\Packages\\66157101-a353-4f28-b29a-ddc6fe58dccc_rvrkc1hdkd6c0\\LocalState\\AlarmSoundFolder\\Horizon.wma",
            //    ToastFilePath = "ms-appdata:///local/Horizon.wma"
            //};



            loadCustmSoundCol(SelectedSound);
            AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles);
            Player = new MediaElement();
            PlaySoundCommand = new RelayCommand<object>(playSound);
            OpenPopUpCommand = new RelayCommand(() => IsPopUpOpen = true);
            PickCustomSoundCommand = new RelayCommand(selectCustomSound);
            SelectedSoundCommand = new RelayCommand<object>(saveNewSelectedSound);
            PopUpUnloadedCommand = new RelayCommand(() => Player.Stop());
        }

        protected override void OnDataLoaded()
        {
            //ToDelete
            // object testObject = null;
            //object testObject = new SoundData() { Title = "0888.wav", FileType = FileTypeEnum.Uri };
            //loadSoundCollections(testObject);
            //testObject = SaveSoundToLocal(testObject);
        }

        private bool IsInLocal(string fileName)
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;
            if (local != null)
            {
                //get the folder in LocalFolder
                var dataFolder = local.GetFolderAsync("AlarmSoundFolder").AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                //get the file
                var file = dataFolder.TryGetItemAsync(fileName).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                return file != null;
            }
            return false;
        }

        /// <summary>
        /// Taking a sound object, checking it's file type
        /// if of type is Custom and still saved in local folder - displaying it's name, loading customSoundCollection + standardSoundCollection
        /// if of type Uri and still saved in local folder- displaying it's name and just loading standardSoundCollection
        /// if nothing found - loading standardSoundCollection with preset sound, and displaying name of a preset sound.
        /// </summary>
        /// <param name="sound"></param>
        private void loadCustmSoundCol(object obj)
        {
            var sound = obj as SoundData;

            //string noExtName = sound.Title.Substring(0, sound.Title.IndexOf("."));

            //if (sound.FileType == FileTypeEnum.Custom && IsInLocal(sound.Title) == true)
            //{
            //    AlarmCustomSoundSelection = new AlarmCustomSoundSelection() { new SoundData { Title = noExtName, FilePath = sound.FilePath, FileType = sound.FileType, IsInLocal = true } };
            //    AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles);
            //    SelectedSoundTitle = noExtName;
            //}
            //else if (sound.FileType == FileTypeEnum.Uri && IsInLocal(sound.Title) == true)
            //{
            //    AlarmCustomSoundSelection = new AlarmCustomSoundSelection();
            //    AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles);
            //    SelectedSoundTitle = noExtName;
            //}
            //else
            //{
            //    SelectedSound = new SoundData() { Title = "Bird Box", FilePath = "ms-appx:/Assets/Ringtones/Bird Box.wma", FileType = FileTypeEnum.Uri };
            //    SelectedSoundTitle = SelectedSound.Title;
            //    AlarmCustomSoundSelection = new AlarmCustomSoundSelection();
            //    AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles);
            //}


            //If no sound or an invalid sound is sent, use the default
            if (sound == null || !IsInLocal(sound.FileName))
            {
                sound = new SoundData() { Title = "Bird Box", FilePath = "ms-appx:///Assets/Ringtones/Bird Box.wma", FileType = FileTypeEnum.Uri, ToastFilePath = "ms-appdata:///local/Bird Box.wma" };
            }

            //Clear out the custom sound collection
            AlarmCustomSoundSelection = new AlarmCustomSoundSelection();

            if (sound.FileType == FileTypeEnum.Custom)
            {
                AlarmCustomSoundSelection.Add(sound);
            }

            //Set the selected sound
            SelectedSound = sound;
            SelectedSoundTitle = sound.Title;


            //if (sound != null)
            //{
            //    string noExtName = sound.Title.Substring(0, sound.Title.IndexOf("."));

            //    switch (sound.FileType)
            //    {
            //        case FileTypeEnum.Uri:
            //            if (IsInLocal(sound.Title))
            //            {
            //                AlarmCustomSoundSelection = new AlarmCustomSoundSelection();
            //                SelectedSoundTitle = noExtName;
            //            }
            //            else
            //            {
            //                goto default;
            //            }
            //            break;

            //        case FileTypeEnum.Custom:
            //            if (IsInLocal(sound.Title))
            //            {
            //                AlarmCustomSoundSelection = new AlarmCustomSoundSelection() { new SoundData { Title = noExtName, FilePath = sound.FilePath, FileType = sound.FileType, IsInLocal = true } };
            //                SelectedSoundTitle = noExtName;
            //            }
            //            else
            //            {
            //                goto default;
            //            }
            //            break;

            //        default:
            //            createSelectedSnd();
            //            break;
            //    }
            //}
            //else
            //{
            //    createSelectedSnd();
            //}
        }

        //private void createSelectedSnd()
        //{
        //    AlarmCustomSoundSelection = new AlarmCustomSoundSelection();
        //    SelectedSound = new SoundData() { Title = "Bird Box", FilePath = "ms-appx:///Assets/Ringtones/Bird Box.wma", FileType = FileTypeEnum.Uri, ToastFilePath= "ms-appdata:///local/Bird Box.wma" };
        //    SelectedSoundTitle = SelectedSound.Title;
        //}

        public async Task<SoundData> SaveSoundToLocal(object soundObject)
        {
            var soundFile = soundObject as SoundData;

          //  if (soundFile!=null&&soundFile.FileType == FileTypeEnum.Uri)
            if (soundFile != null)
            {
                if (soundFile.FileType == FileTypeEnum.Uri)
                {
                    string path = soundFile.FilePath.Replace("ms-appx:/", "ms-appx:///");
                    //if selected sound is from the standard collection, assign CustomStorageFile to it
                    CustomStorageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(path));
                }
            }
            else
            {
                soundFile = new SoundData() { FilePath = SelectedSound.FilePath, Title = SelectedSound.Title, FileType = SelectedSound.FileType, ToastFilePath=SelectedSound.ToastFilePath};
            }

            StorageFolder local = ApplicationData.Current.LocalFolder;
            var dataFolder = await local.CreateFolderAsync("AlarmSoundFolder", CreationCollisionOption.OpenIfExists);
            await CustomStorageFile.CopyAsync(dataFolder, CustomStorageFile.Name, NameCollisionOption.ReplaceExisting);
            var file = dataFolder.TryGetItemAsync(CustomStorageFile.Name).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();

            if (file != null)
            {
                //does not work
                soundFile.Title = (file as StorageFile)?.DisplayName;
                soundFile.ToastFilePath = "ms-appdata:///local/" + file.Name;
                soundFile.FilePath = file.Path;
            }
            // if fails to save, use system sound for toast notification
            else
            {
                soundFile.Title = "SysAlarmSound";
                soundFile.FileType = FileTypeEnum.System;
                soundFile.ToastFilePath = "ms-winsoundevent:Notification.Looping.Alarm";
            }
            return soundFile;
        }

        private async void saveNewSelectedSound(object obj)
        {
            var sound = obj as SoundData;
            SelectedSoundTitle = sound.Title;
           // await SaveSoundToLocal(obj);
            IsPopUpOpen = false;

            //pass it to AlarmPageViewModel
            await SaveSoundToLocal(obj);
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
            else if (sound.FileType == FileTypeEnum.Uri || sound.FileType == FileTypeEnum.Custom && sound.IsInLocal == true)
            {
                playSound(sound.FilePath);
                titlePlayingNow = sound.Title;
            }
            else if (sound.FileType == FileTypeEnum.Custom)
            {
                playCustomSound(sound.Title);
                titlePlayingNow = sound.Title;
            }
        }

        private void playSound(string path)
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
        /// Same song can't be added twice to custom collection
        /// </summary>
        /// <param name="file"></param>
        private void addCustomSong(StorageFile file)
        {
            // if (AlarmCustomSoundSelection.Any(x => x.FilePath == file.Path))
            if (AlarmCustomSoundSelection.Any(x => x.Title == file.DisplayName))
            {
                return;
            }
            else
            {
                AlarmCustomSoundSelection.Add(new SoundData { Title = file.DisplayName, FilePath = file.Path, FileType = FileTypeEnum.Custom, IsInLocal = false });
            }
        }

        /// <summary>
        /// user can pick up a song from diff directories, it adds to AlarmCustomSoundSelection and futureAccess list with it's name as metadata
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
                CustomStorageFile = storageFile;
                addCustomSong(storageFile);
                StorageApplicationPermissions.FutureAccessList.Add(storageFile, storageFile.DisplayName);
            }
        }
    }
}