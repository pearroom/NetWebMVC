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
        static void Main(string[] args)
        {
            SetRoule();
            IHttpServer http = new IHttpServer(roule, interceptor);            
            RunCommand();
        }

        /// <summary>
        /// 路由设置
        /// </summary>
        static void SetRoule()
        {
            Config.AppExe = Application.ExecutablePath;
            Config.RootPath = System.IO.Directory.GetCurrentDirectory();

            //路径,控制器,视图目录,是否拦截(默认true)
            roule.Add("", new IndexController(), "", false);
            roule.Add("Home", new HomeController(), "Home", false);
        }

        /// <summary>
        /// 操作控制
        /// </summary>
        static void RunCommand()
        {
            Console.WriteLine("\"clear\" Clear Page Cache");
            Console.WriteLine("\"show\" Show Page Cache");
            Console.WriteLine("\"exit\" Exit System");
            while (true)
            {
                String msg = Console.ReadLine();
                switch (msg)
                {
                    case "clear":
                        Command.clearPageCache();
                        break;
                    case "show":
                        Command.showPageCache();
                        break;
                    case "exit":
                        System.Environment.Exit(0);
                        break;
                }

            }
        }



    }
}
