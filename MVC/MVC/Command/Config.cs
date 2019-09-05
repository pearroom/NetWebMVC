/*苏兴迎 E-Mail:284238436@qq.com*/
namespace MVC.Command
{
    public class Config
    {
        public static string template = "View";
        public static string template_type = ".html";
        public static string config_file = "resources/config.json";         // 配置文件地址
        public static string mime_file = "resources/mime.json";             // mime配置文件地址
        public static bool open_cache = true;
        public static string cache_max_age = "315360000";
        public static string RootPath = "";
        public static string document_charset = "utf-8";               // 字符集
        public static string SessonName = "jsessionid";
        public static int session_timer = 1;
        public static bool Session_open = true;
        public static string AppExe = "";
        public static string AppName = "";
    }
}
