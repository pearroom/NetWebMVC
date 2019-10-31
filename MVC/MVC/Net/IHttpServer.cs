/*苏兴迎 E-Mail:284238436@qq.com*/
using MVC.Command;
using MVC.MVC.Command;
using System;
using System.IO;
using System.Net;
using System.Threading;
namespace MVC.Net
{

    public class IHttpServer
    {
        public HttpListener Server;
        private static SessionClear sessionClear = new SessionClear();
        private static Interceptor interceptor_;
        public RouleMap Roule { get; set; }
        private bool issuccess = false;
        public IHttpServer(Interceptor interceptor, string ExecutablePath)
        {
            issuccess = true;
            Roule = new RouleMap();
            Config.AppExe = ExecutablePath;
            Config.RootPath = System.IO.Directory.GetCurrentDirectory();
            interceptor_ = interceptor;
            if (!Command.Command.readMime())
            {
                Log.Print("Mime配置文件读取失败");
                issuccess = false;
            }
            if (!Command.Command.readConifg())
            {
                Log.Print("Conifg配置文件读取失败");
                issuccess = false;
            }
        }

        public void Start()
        {
            if (issuccess)
            {
                try
                {
                    string port = Command.Command.configFile["Server"]["Port"].ToString();
                    // string maxThreads = Command.Command.configFile["Server"]["maxThreads"].ToString();
                    Server = new HttpListener();
                    Server.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
                    Server.Prefixes.Add(String.Format("http://+:{0}/", port));
                    Server.Stop();
                    Server.Start();
                    Log.Print("Server Start Port:" + port);
                    Thread.Sleep(1000);
                    new Thread(RunCommand).Start();
                    Server.BeginGetContext(new AsyncCallback(MainProcess), Server);
                }
                catch (Exception e)
                {

                    Log.Print("服务启动[IHttpServer]:" + e.Message);
                }                
            }
            else
            {
                Log.Print("服务启动失败");
            }
        }
        private void RunCommand()
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
                        Command.Command.clearPageCache();
                        break;
                    case "show":
                        Command.Command.showPageCache();
                        break;
                    case "exit":
                        Stop();
                        System.Environment.Exit(0);
                        break;
                }

            }
        }
        public void Stop()
        {
            Server.Stop();
            Server.Close();
        }
        private void MainProcess(IAsyncResult ar)
        {
            HttpListener socket = null;
            HttpListenerContext context = null;
            try
            {
                Server.BeginGetContext(new AsyncCallback(MainProcess), Server);
                socket = ar.AsyncState as HttpListener;
                context = socket.EndGetContext(ar);

            }
            catch (Exception e)
            {

                Log.Print("执行方法[MainProcess]:" + e.Message);
            }

            try
            {
                Command.Command command = new Command.Command();
                command.RunRoule(context, Roule, interceptor_);
            }
            catch (Exception e)
            {

                Log.Print("执行方法[command.RunRoule]:" + e.Message);
            }

        }

    }
}
