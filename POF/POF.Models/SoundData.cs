using GalaSoft.MvvmLight.Messaging;
using POF.Shared;
using System.Text;

namespace POF.Models
{
    public class SoundData : MessageBase
    {
        #region Properties

        private string _filePath;

        public string FilePath
        {
            get { return _filePath; }
            set { _filePath = value; }
        }


        public string FileName => FilePath?.Substring(FilePath.LastIndexOf("\\") + 1);

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }


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



        //public SoundData SetSound()
        //{
        //    //return new SoundData()
        //{
        //    Title = "Archipelago",
        //    FileType = FileTypeEnum.Uri,
        //    ToastFilePath = "ms-appx:///Assets/Ringtones/Archipelago.wma",
        //    FilePath = "C:\\Data\\Users\\DefApps\\AppData\\Local\\Packages\\66157101-a353-4f28-b29a-ddc6fe58dccc_rvrkc1hdkd6c0\\LocalState\\AlarmSoundFolder\\Archipelago.wma"
        //};

        //}


        #endregion Properties
    }
}
