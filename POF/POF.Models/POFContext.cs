using System.Collections.Generic;
using System.Threading.Tasks;

namespace POF.Models
{
    public class POFContext
    {
        public List<SoundFile> StandardSoundFiles { get; private set; }

        //load
        public async Task InitializeContextAsync()
        {
            string basePath = "ms-appx:/Assets/Ringtones/";

            StandardSoundFiles = new List<SoundFile>
            {
                 new SoundFile {Title="Air Display", FilePath=basePath+"Air Display.wma", FileType=Shared.FileTypeEnum.Uri,SongID=1 },
                 new SoundFile {Title="Alablaster", FilePath=basePath+"Alablaster.wma", FileType=Shared.FileTypeEnum.Uri,SongID=2  },
                 new SoundFile {Title="Archipelago", FilePath=basePath+"Archipelago.wma", FileType=Shared.FileTypeEnum.Uri,SongID=3  },
                 new SoundFile {Title="Bird Box", FilePath=basePath+"Bird Box.wma", FileType=Shared.FileTypeEnum.Uri,SongID=4  },
                 new SoundFile {Title="Birds in the Woods", FilePath=basePath+"Birds in the Woods.wma", FileType=Shared.FileTypeEnum.Uri,SongID=5  },
                 new SoundFile {Title="Early Chill", FilePath=basePath+"Early Chill.wma", FileType=Shared.FileTypeEnum.Uri,SongID=6  },
                 new SoundFile {Title="Easy for You", FilePath=basePath+"Easy for You.wma", FileType=Shared.FileTypeEnum.Uri,SongID=7  },
                 new SoundFile {Title="Epic Day", FilePath=basePath+"Epic Day.wma", FileType=Shared.FileTypeEnum.Uri,SongID=8  },
                 new SoundFile {Title="Exoplanet", FilePath=basePath+"Exoplanet.wma", FileType=Shared.FileTypeEnum.Uri,SongID=9  },
                 new SoundFile {Title="Good Times", FilePath=basePath+"Good Times.wma", FileType=Shared.FileTypeEnum.Uri,SongID=10  },
                 new SoundFile {Title="Horizon", FilePath=basePath+"Horizon.wma", FileType=Shared.FileTypeEnum.Uri,SongID=11  },
                 new SoundFile {Title="Lucky Five", FilePath=basePath+"Lucky Five.wma", FileType=Shared.FileTypeEnum.Uri,SongID=12 },
                 new SoundFile {Title="Lumia Clock", FilePath=basePath+"Lumia Clock.wma", FileType=Shared.FileTypeEnum.Uri,SongID=13 },
                 new SoundFile {Title="Miniature of Troy", FilePath=basePath+"Miniature of Troy.wma", FileType=Shared.FileTypeEnum.Uri,SongID=14 },
                 new SoundFile {Title="On the Bridge", FilePath=basePath+"On the Bridge.wma", FileType=Shared.FileTypeEnum.Uri,SongID=15 }
            };

            await Task.FromResult<List<SoundFile>>(StandardSoundFiles);
        }

        public void SaveAlarm()
        {
        }
    }
}