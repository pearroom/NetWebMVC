/*苏兴迎 E-Mail:284238436@qq.com*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace MVC.Command
{
    public class Interceptor
    {
        public HttpListenerRequest Request = null;
        public HttpListenerResponse Response = null;
        public HttpSession Session;
        public Interceptor()
        {

        }
        public void Init(HttpListenerContext context, HttpSession Session_)
        {
            Session = Session_;
            Request = context.Request;
            Response = context.Response;
        }
        public virtual bool isInterceptor()
        {
            return true;
        }
        public void Write(string content)
        {
            if (content == null)
            {
                content = "";
            }
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            Response.ContentLength64 = buffer.Length;
            Response.OutputStream.Write(buffer, 0, buffer.Length);
            Response.OutputStream.Close();
            Response.Close();
        }
    }
}
