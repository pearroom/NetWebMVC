/*苏兴迎 E-Mail:284238436@qq.com*/
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace MVC.Command
{
    public class Compress
    {
        public string CompressType { get; set; }
        public byte[] GZipCompress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }

        public byte[] DeflateCompress(byte[] str)
        {
            var ms = new MemoryStream(str) { Position = 0 };
            var outms = new MemoryStream();
            using (var deflateStream = new DeflateStream(outms, CompressionMode.Compress, true))
            {
                var buf = new byte[1024];
                int len;
                while ((len = ms.Read(buf, 0, buf.Length)) > 0)
                    deflateStream.Write(buf, 0, len);
            }
            return outms.ToArray();
        }
    }
}
