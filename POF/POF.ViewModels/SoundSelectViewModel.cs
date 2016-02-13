
using GalaSoft.MvvmLight.Command;
using POF.Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
           PlaySoundCommand = new RelayCommand<object>(PlaySound);
           OpenPopUpCommand = new RelayCommand(ShowPopUp);
           PickCustomSoundCommand = new RelayCommand(pickUpSound);
           SelectedSoundCommand = new RelayCommand<object>(SaveSelectedSound);
        }


        //TODO: save sound in context, close poup, display sound name on control
        private void SaveSelectedSound(object obj)
        {
            string selectedSound = (obj as SoundData).Title;
            SelectedSoundTitle = selectedSound;
        
            IsPopUpOpen = false;
        }


        private void pickUpSound()
        {
            CustomSoundGroup.Add(StandardSoundGroup[0]);
        }


        private void ShowPopUp()
        {
            IsPopUpOpen = true;
        }


        private void PlaySound(object obj)
        {
           Player.Source = new Uri((obj as SoundData).FilePath, UriKind.RelativeOrAbsolute);
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


        protected override void Save()
        {
            base.Repository.SaveAlarm();
        }
    }
}
