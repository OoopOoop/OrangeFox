using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace SoundChooserToUniversal
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }



    // the model
    public class SoundData:ObservableObject 
    {
        private string filePath;
        private string title;
        private bool isExist;

        [DataMember]
        public string Token { get; set; }

        [DataMember]
        public int SongID { get; set; }

        [DataMember]
        public string FilePath { get { return filePath; } set { filePath = value; NotifyPropertyChanged(); } }

        [DataMember]
        public string Title { get { return title; } set { title = value; NotifyPropertyChanged(); } }
        public bool IsExist { get { return isExist; } set { isExist = value; NotifyPropertyChanged(); } }

       
        [IgnoreDataMember]
        public Windows.Storage.Streams.IRandomAccessStream Stream { get; set; }


        public enum  FileTypeEnum
        {
            Uri,
            Custom
        }

        [DataMember]
        public FileTypeEnum fileType { get; set; }
    }




    //standard list of songs
    public class SoundGroup:ObservableCollection<SoundData>
    { 
        public SoundGroup():base()
        {
            string basePath = "ms-appx:/sounds/";
            Add(new SoundData { Title = "Nevada", FilePath = basePath + "Nevada.mp3", fileType = SoundData.FileTypeEnum.Uri });
            Add(new SoundData { Title = "Ring", FilePath = basePath + "Ring01.wma", fileType = SoundData.FileTypeEnum.Uri });
            Add(new SoundData { Title = "Girl", FilePath = basePath + "That girl from Copenhagen.mp3", fileType = SoundData.FileTypeEnum.Uri });
            Add(new SoundData { Title = "Universe", FilePath = basePath + "Universe.mp3", fileType = SoundData.FileTypeEnum.Uri });
        }
    }



    // custom list of songs that person can choose from various locations on the phone
    public class CustomMusicGroup : ObservableCollection<SoundData>
    {
        public CustomMusicGroup() : base()
        {
            string basePath = "ms-appx:/sounds/";
            Add(new SoundData { Title = "Nevada", FilePath = basePath + "Nevada.mp3", fileType = SoundData.FileTypeEnum.Uri });
            Add(new SoundData { Title = "Ring", FilePath = basePath + "Ring01.wma", fileType = SoundData.FileTypeEnum.Uri });
            Add(new SoundData { Title = "Girl", FilePath = basePath + "That girl from Copenhagen.mp3", fileType = SoundData.FileTypeEnum.Uri });
            Add(new SoundData { Title = "Universe", FilePath = basePath + "Universe.mp3", fileType = SoundData.FileTypeEnum.Uri });
        }

    }














    public class SoundViewModel : ObservableObject
    {
        int customSongID;

        public CustomMusicGroup customMusicGroup;
       


        private string displayDescription="test";
        public string DisplayDescription { get { return displayDescription; } set { displayDescription = value; NotifyPropertyChanged(); }}


        private bool  isVisible=false;
        public bool IsVisible
        {
            get
            {
                return isVisible;
            }
            set
            {
                isVisible = value;
                NotifyPropertyChanged();
            }
        }


        //TODO:
        // loop throught json file to see if any data is saved, if not return empty SoundGroup, otherwice return last saved song.
        // in main assign CustomMusicGroup = SoundModel.GetCustomSounds()
        // in main, to save the last song in click event add SoundModel.SaveCustomLastSong() method
        // add file.ContainingFolderId string, to chech if person trying to add same song to temporary customSoundListview
        // check if song is not deleted from folder, if so, remove from the collection



        const string customSound = "customSound.json";


        public async Task<CustomMusicGroup> GetCustomsounds()
        {
            await EnsureCustomSoundsLoaded();
            return customMusicGroup;
        }



        private async Task EnsureCustomSoundsLoaded()
        {
           // if(customSound.Contains(typeof(SoundData)))


            //check if any song was chosen and saved in json file
            if(!string.IsNullOrEmpty(customSound))
            {
                // if json file contains files typeof SoundData  - deserialize file and put in customSoundListView

               await loadCustomSavedSong();
            }
            else
            {
                // if no file was serialized or customSound file is empty, than retun new empty collection of soundGroup
                customMusicGroup = new CustomMusicGroup();
            }
        }




        public async Task SaveCustomLastSong()
        {
            var JsonSerializer = new DataContractJsonSerializer(typeof(CustomMusicGroup));
            using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForWriteAsync(customSound, CreationCollisionOption.ReplaceExisting))
            {
                JsonSerializer.WriteObject(stream, customMusicGroup);
            }
        }





 
        public void AddCustomSongToCollection(StorageFile file)
        {
            customMusicGroup.Add(new SoundData { SongID = customSongID++, Title = file.DisplayName, FilePath = file.Path, IsExist = true, fileType = SoundData.FileTypeEnum.Custom, Token= "customTest"});
        }




        public async Task loadCustomSavedSong()
        {
            var JsonSerializer = new DataContractJsonSerializer(typeof(CustomMusicGroup));

            try
            {
                using (var stream = await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync(customSound))
                {
                    // deserialize and cast as CustomMusicGroup
                    var test = (CustomMusicGroup)JsonSerializer.ReadObject(stream);




                    //!!!Does not work, sends false
                    // check if music file was not removed
                    if (File.Exists(test[0].FilePath))
                    // or
                    // if(Directory.Exists(Path.GetDirectoryName(test[0].FilePath)))
                    {
                        customMusicGroup = test;
                    }




                    else
                    {
                        // if file was deleted or replaced ---> remove it from the collection  
                        // !!! set default song if custom song was removed
                        // customMusicGroup.Remove(test[0]);
                        customMusicGroup = new CustomMusicGroup();

                        StorageApplicationPermissions.FutureAccessList.Remove(test[0].Token);


                        //remove from json file
                        customSound.Remove(0);

                    }
                }
                
            }




 

            catch
            {
                customMusicGroup = new CustomMusicGroup();
            }
        }





    }
}
