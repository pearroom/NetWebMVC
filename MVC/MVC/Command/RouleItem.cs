/*苏兴迎 E-Mail:284238436@qq.com*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Command
{
    public class RouleItem
    {
        public string path { get; set; }
        public string action { get; set; }
        public string view { get; set; }
        public bool isInterceptor { get; set; }
    }
}
