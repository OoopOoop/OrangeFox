
using GalaSoft.MvvmLight.Command;
using POF.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
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

        private bool _exists;

        public bool Exists
        {
            get { return _exists; }
            set { _exists = value; }
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
        #endregion
    }

    public class SoundSelectViewModel : ViewModelBase
    {

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


        public ICommand OpenPopUpCommand { get; private set;}
        public ICommand PickCustomSoundCommand { get; private set;}
        public ICommand SelectedSoundCommand { get; private set;}
        public ICommand PlaySoundCommand { get; private set;}


        private ObservableCollection<SoundData> _standardSoundGroup;
        public ObservableCollection<SoundData> StandardSoundGroup
        {
            get { return _standardSoundGroup; }
            set { _standardSoundGroup = value; OnPropertyChanged(); }
        }


        private ObservableCollection <SoundData> _customSoundGroup;

        public ObservableCollection <SoundData> CustomSoundGroup
        {
            get { return _customSoundGroup; }
            set { _customSoundGroup = value; }
        }


        private MediaElement _player;
        public MediaElement Player
        {
            get { return _player; }
            set { _player = value; OnPropertyChanged();}
        }



        public SoundSelectViewModel()
        {
           SelectedSoundTitle = "Title";
           Player = new MediaElement();
           PlaySoundCommand = new RelayCommand<object>(playSound);
           OpenPopUpCommand = new RelayCommand(showPopUp);
           PickCustomSoundCommand = new RelayCommand(selectCustomSound);
           SelectedSoundCommand = new RelayCommand<object>(saveSelectedSound);
        }


        //TODO: save sound in context, close poup, display sound name on control
        private void saveSelectedSound(object obj)
        {
            string selectedSound = (obj as SoundData).Title;
            SelectedSoundTitle = selectedSound;
        
            IsPopUpOpen = false;
        }



        private void showPopUp()
        {
            IsPopUpOpen = true;
        }


        private void playSound(object obj)
        {
            var sound = obj as SoundData;

            if(sound ==null)
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


        protected override void OnDataLoaded()
        {
            CustomSoundGroup = new ObservableCollection<SoundData>();
            StandardSoundGroup = new ObservableCollection<SoundData>();
            addSongs();
        }


        private void addSongs()
        {
            foreach (var item in Repository.StandardSoundFiles)
            {
                StandardSoundGroup.Add(new SoundData() { FilePath = item.FilePath, Title=item.Title, Exists=item.Exists, FileType=item.FileType, SongID=item.SongID, Token=item.Token
                });
            }

            //ToDelete

            if(CustomSoundGroup.Count==0)
            {
                CustomSoundGroup.Add(StandardSoundGroup[0]);
                CustomSoundGroup.Add(StandardSoundGroup[1]);
            }
        }


        private void addCustomSong(StorageFile file)
        {
            CustomSoundGroup.Add(new SoundData { Title = file.DisplayName, FilePath = file.Path, Exists = true, FileType = FileTypeEnum.Custom, Token = "custom" });
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

                //var Token = StorageApplicationPermissions.FutureAccessList.Add(storageFile, "customMeta");

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
