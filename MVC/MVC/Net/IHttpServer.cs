/*苏兴迎 E-Mail:284238436@qq.com*/
using MVC.Command;
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
                    string maxThreads = Command.Command.configFile["Server"]["maxThreads"].ToString();
                    Server = new HttpListener();

                    Server.Prefixes.Add(String.Format("http://+:{0}/", port));
                    Server.Start();
                    Server.BeginGetContext(new AsyncCallback(MainProcess), Server);
                    Console.Out.WriteLine("Server Start Port:" + port);
                }
            }
            catch (Exception e)
            {

                Console.Out.WriteLine(e.Message);
            }


        }
        private void MainProcess(IAsyncResult ar)
        {
             HttpListener socket = ar.AsyncState as HttpListener;
            socket.BeginGetContext(new AsyncCallback(MainProcess), socket);
            var context = socket.EndGetContext(ar);
            Command.Command command = new Command.Command();
            command.RunRoule(context, roule_, interceptor_);

        }

    }
}
