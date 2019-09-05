/*苏兴迎 E-Mail:284238436@qq.com*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Command
{
    public class FileItem
    {
        public string FileName { get; set; }
        public byte[] FileStream { get; set; }
        public bool SaveFile(string filepath)
        {
            if (FileStream != null)
                return ByteHelper.WriteByteToFile(FileStream, filepath);
            else
                return false;
        }
    }
}
