/*苏兴迎 E-Mail:284238436@qq.com*/
using MVC.Command;
using MVC.MVC.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace MVC.Net
{

    public class IHttpServer
    {
        public HttpListener Server;
        private static SessionClear sessionClear = new SessionClear();
        private RouleMap roule_;
        private Interceptor interceptor_;
        public IHttpServer(RouleMap roule, Interceptor interceptor)
        {
            try
            {
                roule_ = roule;
                interceptor_ = interceptor;
                if (Command.Command.readMime() && Command.Command.readConifg())
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
                }
            }
            catch (Exception e)
            {

                Log.Print("服务启动[IHttpServer]:" + e.Message);
            }
            Server.BeginGetContext(new AsyncCallback(MainProcess), Server);

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
                command.RunRoule(context, roule_, interceptor_);
            }
            catch (Exception e)
            {

                Log.Print("执行方法[command.RunRoule]:" + e.Message);
            }

        }

    }
}
