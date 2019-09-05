using MVC.Command;
using NetWebMVC.Web.Config;
using NetWebMVC.Web.Controller;
using System.Windows.Forms;

namespace NetWebMVC
{
     class Program
    {
        public static RouleMap roule = new RouleMap();
        public static MyInterceptor interceptor = new MyInterceptor();
        static void Main(string[] args)
        {
            SetConfig();
            SetRoule();
            MVC.Net.IHttpServer http = new MVC.Net.IHttpServer(roule, interceptor);
            
        }

        /// <summary>
        /// 添加路由控制
        /// </summary>
        static void SetRoule()
        {
            //路径,控制器,视图目录,是否拦截(默认true)
            roule.Add("", new IndexController(), "", false);
            roule.Add("Home", new HomeController(), "Home", false);
        }


        /// <summary>
        /// 系统参数设置
        /// </summary>
        static void SetConfig()
        {
            Config.template = "View";                             //视图根目录
            Config.template_type = ".html";                       //模板文件类型
            Config.config_file = "resources/config.json";         //配置文件地址
            Config.mime_file = "resources/mime.json";             //mime配置文件地址
            Config.open_cache = true;                             //启用缓存
            Config.cache_max_age = "315360000";                   //缓存过期时间            
            Config.document_charset = "utf-8";                    //字符集
            Config.SessonName = "jsessionid";                     //session名称
            Config.session_timer = 60;                            //session 过期时间
            Config.Session_open = true;                           //启用sessioin
            Config.AppExe = Application.ExecutablePath;
            Config.RootPath = System.IO.Directory.GetCurrentDirectory();
        }




    }
}
