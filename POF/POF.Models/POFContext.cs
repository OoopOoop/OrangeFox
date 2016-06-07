using System.Collections.Generic;
using System.Threading.Tasks;

namespace POF.Models
{
    public class POFContext
    {
        public List<SoundData> StandardSoundFiles { get; private set; }

        //load
        public async Task InitializeContextAsync()
        {
            string basePath = "ms-appx:/Assets/Ringtones/";

            StandardSoundFiles = new List<SoundData>
            {
                 //new SoundFile {Title="Air Display", FilePath=basePath+"Air Display.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Alablaster", FilePath=basePath+"Alablaster.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Archipelago", FilePath=basePath+"Archipelago.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Bird Box", FilePath=basePath+"Bird Box.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Birds in the Woods", FilePath=basePath+"Birds in the Woods.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Early Chill", FilePath=basePath+"Early Chill.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Easy for You", FilePath=basePath+"Easy for You.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Epic Day", FilePath=basePath+"Epic Day.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Exoplanet", FilePath=basePath+"Exoplanet.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Good Times", FilePath=basePath+"Good Times.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Horizon", FilePath=basePath+"Horizon.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Lucky Five", FilePath=basePath+"Lucky Five.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Lumia Clock", FilePath=basePath+"Lumia Clock.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="Miniature of Troy", FilePath=basePath+"Miniature of Troy.wma", FileType=Shared.FileTypeEnum.Uri},
                 //new SoundFile {Title="On the Bridge", FilePath=basePath+"On the Bridge.wma", FileType=Shared.FileTypeEnum.Uri}

                 new SoundData {Title="Air Display", FilePath=basePath+"Air Display.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Alablaster", FilePath=basePath+"Alablaster.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Archipelago", FilePath=basePath+"Archipelago.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Bird Box", FilePath=basePath+"Bird Box.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Birds in the Woods", FilePath=basePath+"Birds in the Woods.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Early Chill", FilePath=basePath+"Early Chill.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Easy for You", FilePath=basePath+"Easy for You.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Epic Day", FilePath=basePath+"Epic Day.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Exoplanet", FilePath=basePath+"Exoplanet.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Good Times", FilePath=basePath+"Good Times.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Horizon", FilePath=basePath+"Horizon.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Lucky Five", FilePath=basePath+"Lucky Five.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Lumia Clock", FilePath=basePath+"Lumia Clock.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="Miniature of Troy", FilePath=basePath+"Miniature of Troy.wma", FileType=Shared.FileTypeEnum.Uri},
                 new SoundData {Title="On the Bridge", FilePath=basePath+"On the Bridge.wma", FileType=Shared.FileTypeEnum.Uri}
            };

            await Task.FromResult<List<SoundData>>(StandardSoundFiles);
        }

        public void SaveAlarm()
        {
        }
    }
}