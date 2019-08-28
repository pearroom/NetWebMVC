using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebMVC.Web.Controller
{
    class HomeController:MVC.Command.Controller
    {
        public void Index()
        {
            //JArray ja = new JArray();
            //JObject jo = new JObject();
            //jo.Add("name", "你好");
            //ja.Add(jo);
            // ShowJSON(ja);
            ShowHTML("index");

        }
        public void say()
        {

            ShowJSON("ok");
        }
    }
}
