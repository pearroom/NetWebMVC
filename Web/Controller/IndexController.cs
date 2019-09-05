
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Newtonsoft.Json;
using System.Threading;
using NetWebMVC.Web.Config;

namespace NetWebMVC.Web.Controller
{
    class IndexController : MVC.Command.Controller
    {
        public void Index()
        {

            //JObject jo = new JObject();
          // jo["account"] = "aaaaaa";
        //   ShowText(jo.ToString());
              ShowText("hello");    
           // if(isPOST())
          //  ShowHTML("login");
        }
        public void Say(string name, int age, float a, double b, DateTime dd)
        {
            string s = InputById(1);
            string s1 = Input("name");
            ShowText(name + "age:" + age + "a:" + a + "b:" + b + "date:" + dd.ToShortDateString());
            //  var list = DB.Context.From<tb_order>().ToDataSet();
            // ShowJSON(list);
            // ShowText("hello");
        }
        public void Show()
        {
            string name = Input("name");
            //  SetAttr("name", "大家好");
            var list = DB.Context.From<tb_order>().ToDataTable() ;
            // SetAttr("list", list);
            //   var one = DB.Context.From<tb_project>().First();
            //   SetAttr("project", one);
            // ShowHTML("index");
            ShowJSON(list);
        }
        public void check()
        {
            string txt = Content();
            string name = Input("name");
            ShowText(name);
        }
        public void upimage()
        {
            string filename = Files().FileName;
            Files().SaveFile("D:\\" + filename);
            ShowText("ok");
        }
    }
}
