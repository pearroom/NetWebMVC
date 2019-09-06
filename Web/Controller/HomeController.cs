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
            ShowHTML("index");

        }
        public void say()
        {

            ShowJSON("ok");
        }
    }
}
