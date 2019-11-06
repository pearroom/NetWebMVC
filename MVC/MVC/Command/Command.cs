/*苏兴迎 E-Mail:284238436@qq.com*/
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace MVC.Command
{
    public class Command
    {
        public static JArray MIME;
        public static JObject configFile;
        private static Assembly assembly = Assembly.LoadFrom(Config.AppExe);
        public static Compress compress = new Compress();
        private static Hashtable directory_permission = new Hashtable();
        private bool check_directory_permission(string url)
        {
            url = url.ToLower();
            foreach (string keytmp in directory_permission.Keys)
            {
                string key = keytmp.ToLower();
                if (url.Length >= key.Length)
                {
                    if (url.Substring(0, key.Length).Equals(key))
                    {
                        return (bool)directory_permission[key];
                    }
                }
            }
            return true;
        }
        public static void clearPageCache()
        {
            lock (Controller.pageCacheList.SyncRoot)
            {
                Controller.pageCacheList.Clear();
                Console.WriteLine("Clear Finally");
            }

        }
        public static void showPageCache()
        {
            int k = 1;
            lock (Controller.pageCacheList.SyncRoot)
            {
                foreach (var key in Controller.pageCacheList.Keys)
                {
                    Console.WriteLine(k + ":" + key.ToString());
                    k++;
                }
                Console.WriteLine("Finally");
            }


        }
        public void RunRoule(HttpListenerContext context, RouleMap roule, Interceptor interceptor_)
        {
            if (context == null)
            {
                Log.Print("链接还未建立");
                return;
            }
            try
            {

                string path = context.Request.RawUrl.Trim();
                path = Uri.UnescapeDataString(path);
                //  Log.Print(path);
                string url = path;
                string methodname = "";
                string url_suffix = "";
                if (!check_directory_permission(path))
                {
                    Error404(context, url);
                    return;
                }

                if (isMimeType(path) == null)
                {
                    string[] m = path.Split('/');

                    RouleItem action = null;
                    action = roule.getRoule(path, ref methodname, ref url_suffix);

                    if (action != null)
                    {
                        if (methodname.ToLower().Equals("index") && url[url.Length - 1] != '/')
                        {
                            context.Response.StatusCode = 301;
                            context.Response.RedirectLocation = url + "/";
                            context.Response.AddHeader("Cache-Control", "no-cache,no-store");
                            Write(context, "");
                            return;
                        }

                        HttpSession Session = null;
                        bool isInterceptor = false;
                        if (Config.Session_open)
                        {
                            Session = new HttpSession(context);
                        }
                        if (action.isInterceptor)
                        {
                            object Interceptor_obj = assembly.CreateInstance(interceptor_.ToString()); // 创建类的实例 
                            Type Interceptor_type = Interceptor_obj.GetType();
                            MethodInfo Interceptor_method = Interceptor_type.GetMethod("Init", new Type[] { typeof(HttpListenerContext), typeof(HttpSession) });
                            object[] parameters1 = new object[] { context, Session };
                            Interceptor_method.Invoke(Interceptor_obj, parameters1);
                            Interceptor_method = Interceptor_type.GetMethod("isInterceptor");
                            isInterceptor = (bool)Interceptor_method.Invoke(Interceptor_obj, null);
                        }

                        if (!isInterceptor)
                        {

                            object obj = assembly.CreateInstance(action.action); // 创建类的实例 
                            Type type = obj.GetType();
                            MethodInfo method_main = type.GetMethod(methodname);
                            if (method_main == null)
                            {
                                method_main = type.GetMethod(methodname.ToLower());
                            }
                            MethodInfo method = type.GetMethod("Init", new Type[] { typeof(HttpListenerContext), typeof(HttpSession), typeof(string), typeof(string), typeof(Compress) });
                            object[] parameters = new object[] { context, Session, action.view, url_suffix, compress };

                            try
                            {
                                method.Invoke(obj, parameters);
                            }
                            catch (Exception e)
                            {

                                Log.Print("[Init]方法执行异常:" + path + "|" + e.Message + "|" + method.ToString());
                                Error404(context, url);
                            }

                            if (method_main != null)
                            {
                                object[] parameters1 = null;
                                try
                                {
                                    parameters1 = formatParam(context, method_main, url_suffix);
                                }
                                catch (Exception e)
                                {

                                    Log.Print("[formatParam]异常:" + path + "|" + e.Message);
                                    parameters1 = null;
                                }
                                try
                                {
                                    method_main.Invoke(obj, parameters1);
                                    return;
                                }
                                catch (Exception e)
                                {

                                    Log.Print("方法执行异常:" + path + "|" + e.Message + "|" + method_main.ToString());

                                }


                                if (context.Response.ContentLength64 == 0)
                                {
                                    Error404(context, url);
                                }
                            }
                            else
                            {
                                Error404(context, url);
                            }
                        }

                    }
                    else
                    {
                        Error404(context, url);
                    }

                }
                else
                {
                    if (Config.open_cache && !Config.open_debug)
                    {
                        DateTime n = DateTime.Now;
                        string Modified_time = n.ToUniversalTime().ToString("r");
                        string Expires_time = n.AddHours(24).ToUniversalTime().ToString("r");
                        context.Response.AddHeader("Cache-Control", "max-age=" + Config.cache_max_age);
                        context.Response.AddHeader("Pragma", "Pragma");
                        context.Response.AddHeader("Last-Modified", Modified_time);
                        context.Response.AddHeader("Expires", Expires_time);
                    }
                    else
                    {
                        context.Response.AddHeader("Cache-Control", "no-cache,no-store");
                    }
                    string path2 = path;
                    if (!string.IsNullOrEmpty(Config.AppName))
                    {
                        string tmp = "/" + Config.AppName + "/";
                        if (!path.Substring(0, tmp.Length).Equals(tmp) && !path.Equals("/favicon.ico"))
                        {
                            Error404(context, url);
                            return;
                        }
                        else
                        {
                            path2 = path.Replace(tmp, "/");
                        }
                    }
                    string root = "";
                    if (!Config.WebRoot.Equals(""))
                    {
                        root = "/" + Config.WebRoot;
                    }

                    string htmlfile = System.IO.Directory.GetCurrentDirectory() + root + path2;
                    if (htmlfile.IndexOf('?') > -1)
                    {
                        htmlfile = htmlfile.Substring(0, htmlfile.IndexOf('?'));
                    }
                    if (File.Exists(htmlfile))
                    {
                        string extension = System.IO.Path.GetExtension(htmlfile).Replace(".", "");

                        byte[] data = File.ReadAllBytes(htmlfile);
                        string mime = getMimeType(extension);
                        context.Response.ContentType = mime;
                        context.Response.StatusCode = 200;
                        WriteByte(context, data);
                    }
                    else
                    {
                        Error404(context, path);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Print("路由方法[RunRoule]:" + e.Message);
                Error500(context, e.Message);
            }

        }
        private object[] formatParam(HttpListenerContext context, MethodInfo method, string url_suffix)
        {
            //  HttpListenerContext context = _context_ as HttpListenerContext;
            string[] urlparams = null;
            ParameterInfo[] parames = method.GetParameters();
            object[] parameters = new object[parames.Length];
            if (context.Request.HttpMethod.Equals("GET"))
            {
                if (url_suffix.IndexOf('?') > 0 && parames.Length > 0)
                {
                    url_suffix = url_suffix.Substring(url_suffix.IndexOf('?') + 1);
                    urlparams = url_suffix.Split('&');
                    //  if (urlparams.Length == parames.Length)
                    {
                        foreach (string paramv in urlparams)
                        {
                            string[] ss = paramv.Split('=');
                            for (int i = 0; i < parames.Length; i++)
                            {
                                if (parames[i].Name.Equals(ss[0]))
                                {
                                    string name = parames[i].Name;
                                    Type paramType = parames[i].ParameterType;
                                    parameters[i] = getParamValue(paramType, ss[1]);
                                    break;
                                }
                            }
                        }

                    }
                }
                else
                {

                    if (parames.Length > 0)
                    {
                        urlparams = url_suffix.Split('/');
                        //  if (urlparams.Length == parames.Length + 1)
                        {

                            for (int i = 0; i < parames.Length; i++)
                            {
                                if (urlparams.Length > 1 && i < urlparams.Length - 1)
                                {
                                    string name = parames[i].Name;
                                    Type paramType = parames[i].ParameterType;
                                    parameters[i] = getParamValue(paramType, urlparams[i + 1]);
                                }


                            }
                        }
                    }
                }
            }
            return parameters;
        }
        private object getParamValue(Type paramType, string value)
        {
            object ret = null;
            if (paramType.Name.Equals("String"))
            {
                ret = value;
            }
            else if (paramType.Name.Equals("Int32"))
            {
                try
                {
                    ret = Convert.ToInt32(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToInt32("0");
                }
            }
            else if (paramType.Name.Equals("Int16"))
            {
                try
                {
                    ret = Convert.ToInt16(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToInt32("0");
                }
            }
            else if (paramType.Name.Equals("Int64"))
            {
                try
                {
                    ret = Convert.ToInt64(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToInt32("0");
                }
            }
            else if (paramType.Name.Equals("Double"))
            {
                try
                {
                    ret = Convert.ToDouble(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToDouble(0);
                }
            }
            else if (paramType.Name.Equals("DateTime"))
            {
                try
                {
                    ret = Convert.ToDateTime(value);
                }
                catch (Exception)
                {

                    ret = null;
                }
            }
            else if (paramType.Name.Equals("Single"))
            {
                try
                {
                    ret = Convert.ToSingle(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToSingle(0);
                }
            }
            else if (paramType.Name.Equals("float"))
            {
                try
                {
                    ret = Convert.ToSingle(value);
                }
                catch (Exception)
                {

                    ret = Convert.ToSingle(0);
                }
            }

            return ret;
        }
        public void Write(HttpListenerContext context_, string content)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            WriteByte(context_, buffer);
        }
        private static void WriteByte(HttpListenerContext context_, byte[] buffer)
        {

            byte[] sdata = null;
            if (compress.CompressType.ToLower().Equals("deflate"))
            {
                context_.Response.AddHeader("Content-Encoding", "deflate");
                sdata = compress.DeflateCompress(buffer);
            }
            else if (compress.CompressType.ToLower().Equals("gzip"))
            {
                context_.Response.AddHeader("Content-Encoding", "gzip");
                sdata = compress.GZipCompress(buffer);
            }
            else
            {
                sdata = buffer;
            }
            context_.Response.ContentLength64 = sdata.Length;
            context_.Response.OutputStream.Write(sdata, 0, sdata.Length);
            context_.Response.OutputStream.Close();
            context_.Response.Close();
        }
        private void Error404(HttpListenerContext context_, string url)
        {
            string s = "";
            context_.Response.StatusCode = 404;
            context_.Response.ContentType = "text/html; charset=" + Config.document_charset;
            s = "<html><body><div style=\"text-align: left;\">";
            s = s + "<div><h1>Error 404</h1></div>";
            s = s + "<hr><div>[ " + url + " ] Not Find Page </div></div></body></html>";
            if (!Config.Error404.Equals(""))
            {
                string htmlfile = Config.Error404;
                if (File.Exists(htmlfile))
                {
                    s = File.ReadAllText(htmlfile);
                }
            }
            Write(context_, s);
        }
        private void Error500(HttpListenerContext context_, string Error)
        {
            string s = "";
            context_.Response.StatusCode = 500;
            context_.Response.ContentType = "text/html; charset=" + Config.document_charset;
            s = "<html><body><div style=\"text-align: left;\">";
            s = s + "<div><h1>Error 500</h1></div>";
            s = s + "<hr><div> " + Error + " </div></div></body></html>";
            if (!Config.Error500.Equals(""))
            {
                string htmlfile = Config.Error500;
                if (File.Exists(htmlfile))
                {
                    s = File.ReadAllText(htmlfile);
                }
            }
            Write(context_, s);
        }
        private static void setConfig(JObject param)
        {

            if (param["Config"] != null)
            {
                JObject jo = param["Config"].ToObject<JObject>();
                if (jo["AppName"] != null)
                    Config.AppName = jo["AppName"].ToString();
                if (jo["WebRoot"] != null)
                    Config.WebRoot = jo["WebRoot"].ToString();
                if (jo["template"] != null)
                    Config.template = jo["template"].ToString();
                if (jo["template_type"] != null)
                    Config.template_type = jo["template_type"].ToString();
                if (jo["open_cache"] != null)
                    Config.open_cache = jo["open_cache"].ToObject<bool>();
                if (jo["document_charset"] != null)
                    Config.document_charset = jo["document_charset"].ToString();
                if (jo["SessonName"] != null)
                    Config.SessonName = jo["SessonName"].ToString();
                if (jo["session_timer"] != null)
                    Config.session_timer = jo["session_timer"].ToObject<int>();
                if (jo["Session_open"] != null)
                    Config.Session_open = jo["Session_open"].ToObject<bool>();
                if (jo["open_debug"] != null)
                    Config.open_debug = jo["open_debug"].ToObject<bool>();
                if (jo["Error404"] != null)
                    if (!jo["Error404"].ToObject<string>().Trim().Equals(""))
                        Config.Error404 = Config.WebRoot + "/" + jo["Error404"].ToObject<string>();
                if (jo["Error500"] != null)
                    if (!jo["Error500"].ToObject<string>().Trim().Equals(""))
                        Config.Error500 = Config.WebRoot + "/" + jo["Error500"].ToObject<string>();
                if (jo["JsonToLower"] != null)
                    Config.JsonToLower = jo["JsonToLower"].ToObject<bool>();
                if (jo["directory"] != null)
                {
                    JArray array = jo["directory"].ToObject<JArray>();
                    foreach (var item in array)
                    {
                        string path = item["path"].ToString();
                        bool permission = item["permission"].ToObject<bool>();
                        directory_permission.Add(path, permission);
                    }
                }

            }
        }
        public static bool readConifg()
        {
            string file = Config.RootPath + "/" + Config.config_file;
            StringBuilder html = new StringBuilder();
            if (File.Exists(file))
            {
                try
                {
                    html.Append(File.ReadAllText(file));
                    configFile = JObject.Parse(html.ToString());
                    compress.CompressType = configFile["Server"]["Compress"].ToString();
                    setConfig(configFile);
                }
                catch (Exception)
                {

                    return false;
                }

                return true;
            }
            else
                return false;
        }
        public static bool readMime()
        {
            string mimefile = Config.RootPath + "/" + Config.mime_file;
            StringBuilder html = new StringBuilder();
            if (File.Exists(mimefile))
            {
                try
                {
                    html.Append(File.ReadAllText(mimefile));
                    MIME = JArray.Parse(html.ToString());
                }
                catch (Exception)
                {

                    return false;
                }

                return true;
            }
            else
                return false;

        }
        public static string getMimeType(string type)
        {
            foreach (var jo in MIME)
            {
                if (type.ToLower().Equals(jo["Extensions"].ToString().ToLower()))
                {
                    return jo["MimeType"].ToString() + "; charset=utf-8";
                }
            }
            return "";
        }
        public static string isMimeType(string path)
        {
            if (path.IndexOf('?') > -1)
            {
                path = path.Substring(0, path.IndexOf('?'));
            }
            foreach (var jo in MIME)
            {
                string aLastName = path.Substring(path.LastIndexOf(".") + 1, (path.Length - path.LastIndexOf(".") - 1));
                string mime = jo["Extensions"].ToString().ToLower();
                if (mime.IndexOf(aLastName) > -1)
                {
                    return jo["MimeType"].ToString() + "; charset=utf-8";
                }
            }
            return null;
        }
    }

}