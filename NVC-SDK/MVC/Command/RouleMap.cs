/*苏兴迎 E-Mail:284238436@qq.com*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVC.Command
{
    public class RouleMap
    {
        List<RouleItem> items = new List<RouleItem>();
        /// <summary>
        /// 添加路由及对应控制器
        /// </summary>
        /// <param name="path">路由</param>
        /// <param name="controller">控制器</param>
        /// <param name="view">视图目录</param>
        /// <param name="isInterceptor">是否拦截(默认true拦截)</param>
        public void Add(string path, Controller controller, string view, bool isInterceptor = true)
        {
            if (path == "")
                path = "/";
            else
                path = "/" + path + "/";
            string s = controller.ToString();
            RouleItem it = new RouleItem();
            it.action = s;
            it.path = path;
            it.view = view;
            it.isInterceptor = isInterceptor;
            items.Add(it);
            items = items.OrderByDescending(c => c.path).ToList();
        }

        public RouleItem getRoule(string path, ref string methodname, ref string restfull)
        {
            string url = path;
            if (url.IndexOf('?') > 0)
            {
                url=url.Substring(0, url.IndexOf('?'));
            }
            foreach (var item in items)
            {
                string tmps = "";
                if (item.path.Length > url.Length)
                    tmps = url;
                else
                    tmps = url.Substring(0, item.path.Length);
                if (item.path.Equals(tmps) || item.path.Equals(tmps + "/"))
                {
                    restfull = path.Substring(tmps.Length);
                    string m = url.Substring(tmps.Length);
                    string[] ss = m.Split('/');
                    methodname = ss[0];
                    if (methodname.Equals(""))
                        methodname = "Index";
                    return item;

                }


            }
            return null;

        }
    }
}
