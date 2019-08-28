using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetWebMVC.Web.Config
{
    class MyInterceptor:MVC.Command.Interceptor
    {
        public override bool isInterceptor()
        {
            if (Session.Get("name") == null)
            {
                string s = Session.Get("name").ToString();
                this.Write("拦截了");
                return true;
            }
            else
            {
                return false;
            }
       
            
        }
    }
}
