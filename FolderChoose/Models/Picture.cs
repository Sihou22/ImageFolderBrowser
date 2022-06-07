using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace FolderChoose.Models
{
   public class Picture
    {
        public string Name { get; set; }
        public StorageFile PicFile { get; set; }
        public string FileType { get; set; }
        public string Path { get; set; }
    }
}
