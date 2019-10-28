/*苏兴迎 E-Mail:284238436@qq.com*/
using MVC.Command;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MVC.Command
{
    public class SessionClear
    {
        public SessionClear()
        {
            Thread th = new Thread(checkSession);
            th.Start();
        }
        private void checkSession()
        {

            while (true)
            {
                try
                {
                    IDictionaryEnumerator item;
                    IDictionaryEnumerator item1;
                    Hashtable sessionList;
                    Hashtable sessionMap;
                 //   lock (HttpSession.sessionList.SyncRoot)
                    {
                        sessionList = (Hashtable)HttpSession.sessionList.Clone();
                    }
                  //  lock (HttpSession.sessionMap)
                    {
                        sessionMap = (Hashtable)HttpSession.sessionMap.Clone();
                    }                    
                    item = sessionList.GetEnumerator();
                    while (item.MoveNext())
                    {
                        string key = (string)item.Key;
                        DateTime time_out = (DateTime)sessionList[key];
                        if (time_out.AddMinutes(Config.session_timer) < DateTime.Now)
                        {
                            item1 = sessionMap.GetEnumerator();
                            while (item1.MoveNext())
                            {
                                string suffkey = Config.SessonName + "_" + key;
                                if (suffkey.Equals(item1.Key.ToString().Substring(0, suffkey.Length)))
                                {
                                    //  Console.Out.WriteLine("清理:" + item1.Key.ToString());
                                    HttpSession.sessionMap.Remove(item1.Key.ToString());

                                }
                                Thread.Sleep(10);
                            }
                            //Console.Out.WriteLine("清理:" + key);
                            HttpSession.sessionList.Remove(key);

                        }

                        Thread.Sleep(10);
                    }
                    sessionList.Clear();
                    sessionMap.Clear();
                    sessionList = null;
                    sessionMap = null;
                    item = null;
                    item1 = null;
                }
                catch (Exception e)
                {

                    Console.Out.WriteLine(e.Message);
                }
                finally
                {

                    Thread.Sleep(1000);
                }


            }
        }
    }
}
