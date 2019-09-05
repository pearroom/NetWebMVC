/*苏兴迎 E-Mail:284238436@qq.com*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Collections;
using MVC;

namespace MVC.Command
{
    public class HttpSession
    {
        private string SessionID = "";
        //   public static Hashtable sessionMap = System.Collections.Hashtable.Synchronized(new Hashtable());
        //  public static Hashtable sessionList =  System.Collections.Hashtable.Synchronized(new Hashtable());
        public static Hashtable sessionMap = new Hashtable();
        public static Hashtable sessionList = new Hashtable();

        public HttpSession(HttpListenerContext context)
        {
            bool isAdd = true;
            Cookie cook = context.Request.Cookies[Config.SessonName];
            if (cook == null||string.IsNullOrEmpty(cook.Value))
            {
                SessionID = Guid.NewGuid().ToString("N");
                cook = new Cookie();
            }
            else
            {
                SessionID = cook.Value;
                isAdd = false;
            }
            cook.Name = Config.SessonName;
            cook.Path = "/";
            cook.Value = SessionID;
            cook.Expires = DateTime.Now.AddMinutes(Config.session_timer);
            if (isAdd)
            {
                context.Response.AppendCookie(cook);
            }
            else
            {
                context.Response.SetCookie(cook);
            }
            if (!sessionList.ContainsKey(SessionID))
            {
                sessionList.Add(SessionID, DateTime.Now.AddMinutes(Config.session_timer));
            }
            else
            {
                sessionList[SessionID] = DateTime.Now.AddMinutes(Config.session_timer);
            }



        }
        public void Set(string key, object value)
        {
            key = Config.SessonName + "_" + SessionID + "_" + key;
            if (!sessionMap.ContainsKey(key))
                sessionMap.Add(key, value);
            else
                sessionMap[key] = value;

        }
        public object Get(string key)
        {
            key = Config.SessonName + "_" + SessionID + "_" + key;
            return sessionMap[key];
        }
        public void Remove(string key)
        {
            key = Config.SessonName + "_" + SessionID + "_" + key;
            sessionMap.Remove(key);
            sessionList.Remove(key);
        }
        public int Count()
        {
            return sessionList.Count;
        }


    }
}
