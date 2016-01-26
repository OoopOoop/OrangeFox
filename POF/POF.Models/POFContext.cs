﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.Models
{
   public class POFContext
    {
        public List<SoundFile> StandardSoundFiles { get; private set;}

        public async Task InitializeContextAsync()
        {
            //await Task.Delay(2000);

            string basePath = "ms-appx:/Sounds/";

            StandardSoundFiles = new List<SoundFile>
            {
                 new SoundFile {Title="Nevada", FilePath=basePath+"Nevada.mp3", FileType=Shared.FileTypeEnum.Uri },
                 new SoundFile {Title="Ring", FilePath=basePath+"Ring01.wma", FileType=Shared.FileTypeEnum.Uri },
                 new SoundFile {Title="Girl", FilePath=basePath+"That girl from Copenhagen.mp3", FileType=Shared.FileTypeEnum.Uri },
                 new SoundFile {Title="Universe", FilePath=basePath+"Universe.mp3", FileType=Shared.FileTypeEnum.Uri }
            };
        }
    }
}
