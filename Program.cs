using MVC.Command;
using MVC.Net;
using NetWebMVC.Web.Config;
using NetWebMVC.Web.Controller;
using System;
using System.Windows.Forms;

namespace NetWebMVC
{
    class Program
    {
        public static RouleMap roule = new RouleMap();
        public static MyInterceptor interceptor = new MyInterceptor();
        public static IHttpServer httpserver;
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProcessExit);
            Config.AppExe = Application.ExecutablePath;
            Config.RootPath = System.IO.Directory.GetCurrentDirectory();
            httpserver = new IHttpServer(roule, interceptor);
            SetRoule();
        }
        static void SetRoule()
        {
            //路径,控制器,视图目录,是否拦截(默认true)
            roule.Add("", new IndexController(), "", false);
            roule.Add("Home", new HomeController(), "Home", false);
        }
        static void ProcessExit(object sender, EventArgs e)
        {
            httpserver.Stop();
        }
    }
}
