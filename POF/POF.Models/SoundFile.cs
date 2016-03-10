using POF.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POF.Models
{
   public class SoundFile
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Token { get; set; }
        public int SongID { get; set; }
        public FileTypeEnum FileType { get; set;}

        public Windows.Storage.Streams.IRandomAccessStream Stream { get; set; }
    }
}
