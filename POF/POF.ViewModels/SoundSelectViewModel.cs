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

        private string _token;

        public string Token
        {
            get { return _token; }
            set { _token = value; }
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

        public AlarmStandardSoundSelection(List<SoundFile> modelSoundCollection, string selectedSoundPath) : this()
        {
            this.addSounds(modelSoundCollection);
            SelectedSoundByPath(selectedSoundPath);
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
                this.Add(new SoundData() { FilePath = item.FilePath, Title = item.Title, FileType = item.FileType, SongID = item.SongID, Token = item.Token, });
            }
        }

        public string SelectedSoundByPath(string path)
        {
            var selectable = from i in this.Items.Cast<SoundData>()
                             where i.FilePath == path
                             select i;

            foreach (var item in selectable)
            {
                SelectedSoundTitle = item.Title;
            }

            return SelectedSoundTitle;
        }
    }

    public class AlarmCustomSoundSelection : ObservableCollection<SoundData>
    {
        public string SelectedSoundTitle { get; set; }

        public AlarmCustomSoundSelection(string filePath) : this()
        {
            try
            {
                //Task.Run(() => FindSoundFile(filePath)).Wait();
                StorageFile file = StorageFile.GetFileFromPathAsync(filePath).AsTask().ConfigureAwait(false).GetAwaiter().GetResult();
                this.Add(new SoundData() { Title = file.DisplayName, FilePath = file.Path, FileType = FileTypeEnum.Custom, Token = "custom" });
                SelectedSoundTitle = file.DisplayName;
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

    public class SelectedSound
    {
        public string Path { get; set; }
        public FileTypeEnum FileType { get; set; }
    }

    public class SoundSelectViewModel : ViewModelBase
    {
        public List<object> test { get; set; }

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

        public ICommand OpenPopUpCommand { get; }
        public ICommand PickCustomSoundCommand { get; }
        public ICommand SelectedSoundCommand { get; }
        public ICommand PlaySoundCommand { get; }
        public ICommand PopUpUnloadedCommand { get; }

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

        public SelectedSound SelectedSound { get; set; }

        private const string preSetSoundPath = "ms-appx:/Assets/Ringtones/Good Times.wma";

        public SoundSelectViewModel()
        {
            // SelectedSound = new SelectedSound() {Path="ms - appx:/Assets/Ringtones/Horizon.wma", FileType=FileTypeEnum.Uri};
            SelectedSound = new SelectedSound() { Path = "C:\\Data\\Users\\Public\\ Music\\TestFile .wav", FileType = FileTypeEnum.Custom };

            SelectedSound.Path = Regex.Replace(SelectedSound.Path, @"\s+", "");
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
                AlarmCustomSoundSelection = new AlarmCustomSoundSelection(SelectedSound.Path);
                if (AlarmCustomSoundSelection.Count != 0)
                {
                    AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles);
                    SelectedSoundTitle = AlarmCustomSoundSelection.SelectedSoundTitle;
                }
                else
                {
                    AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles, preSetSoundPath);
                    SelectedSoundTitle = AlarmStandardSoundSelection.SelectedSoundTitle;
                }
            }
            else
            {
                AlarmCustomSoundSelection = new AlarmCustomSoundSelection();
                AlarmStandardSoundSelection = new AlarmStandardSoundSelection(base.Repository.StandardSoundFiles, SelectedSound.Path);
                SelectedSoundTitle = AlarmStandardSoundSelection.SelectedSoundTitle;
            }
        }

        private void saveNewSelectedSound(object obj)
        {
            var sound = obj as SoundData;
            SelectedSound.Path = sound.FilePath;
            SelectedSound.FileType = sound.FileType;

            SelectedSoundTitle = sound.Title;
            IsPopUpOpen = false;
        }

        private void playSound(object obj)
        {
            var sound = obj as SoundData;

            if (sound == null)
            {
                Player.Stop();
                return;
            }

            // check if player playing same song when play clicked
            if (Player.CurrentState == Windows.UI.Xaml.Media.MediaElementState.Playing && Player.Source.AbsoluteUri == sound.FilePath)
            {
                Player.Stop();
            }
            else if (sound.FileType == FileTypeEnum.Uri)
            {
                playStandardsound(sound.FilePath);
            }
            else if (sound.FileType == FileTypeEnum.Custom)
            {
                playCustomSound();
            }
        }

        private void playStandardsound(string path)
        {
            Player.Source = new Uri(path, UriKind.RelativeOrAbsolute);
        }

        private async void playCustomSound()
        {
            foreach (AccessListEntry item in StorageApplicationPermissions.FutureAccessList.Entries)
            {
                if (item.Metadata == "customSoundMeta")
                {
                    StorageFile actualFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(item.Token);
                    var stream = await actualFile.OpenAsync(FileAccessMode.Read);
                    Player.SetSource(stream, actualFile.ContentType);
                }
            }
        }

        private void addCustomSong(StorageFile file)
        {
            AlarmCustomSoundSelection.Add(new SoundData { Title = file.DisplayName, FilePath = file.Path, FileType = FileTypeEnum.Custom, Token = "custom" });
        }

        async private void selectCustomSound()
        {
            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.HomeGroup;
            filePicker.ViewMode = PickerViewMode.List;

            filePicker.FileTypeFilter.Clear();

            filePicker.FileTypeFilter.Add(".mp3");
            filePicker.FileTypeFilter.Add(".wav");
            filePicker.FileTypeFilter.Add(".wma");
            filePicker.FileTypeFilter.Add(".qcp");
            filePicker.FileTypeFilter.Add(".aac");

            StorageFile storageFile = await filePicker.PickSingleFileAsync();

            if (storageFile != null)
            {
                var stream = await storageFile.OpenAsync(FileAccessMode.Read);

                addCustomSong(storageFile);

                StorageApplicationPermissions.FutureAccessList.Add(storageFile, "customSoundMeta");
            }
        }

        //Moove to a NewAlarm viewModel, where it looks what sound, time and day was chosen and saves it.
        //protected override void Save()
        //{
        //    base.Repository.SaveAlarm();
        //}
    }
}