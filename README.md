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
			public static IHttpServer httpserver;
			static void Main(string[] args)
			{
				AppDomain.CurrentDomain.ProcessExit += new EventHandler(ProcessExit);
				httpserver = new IHttpServer(new MyInterceptor(), Application.ExecutablePath);
				//路径,控制器,视图目录,是否拦截(默认true)
				httpserver.Roule.Add("", new IndexController(), "", false);
				httpserver.Roule.Add("Home", new HomeController(), "Home");
				httpserver.Start();
			}
			static void ProcessExit(object sender, EventArgs e)
			{
				httpserver.Stop();
			}
		}
	}
